using System;
using System.IO;
using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoDS1307
{
    public class DS1307
    {
        private const ushort Address = 0x68;
        private const int ClockRate = 100;
        private const int Timeout = 1000;

        private const byte SecondsAddr = 0x00;
        private const byte MinutesAddr = 0x01;
        private const byte HoursAddr = 0x02;
        //private const byte DayAddr = 0x03;
        private const byte DateAddr = 0x04;
        private const byte MonthAddr = 0x05;
        private const byte YearAddr = 0x06;
        private const byte ControlAddr = 0x07;
        private const byte RamAddr = 0x08;
        private const byte RamLength = 56;
        private const byte OkMarker = 123; //niewazne jaka wartosc

        public enum Rate
        {
            Freq1Hz = 0x00,
            Freq4K096Hz = 0x01,
            Freq8K192Hz = 0x02,
            Freq32K768Hz = 0x03
        }

        private readonly I2CDevice _device;

        public DS1307()
        {
            var config = new I2CDevice.Configuration(Address, ClockRate);
            _device = new I2CDevice(config);
        }

        public bool Running()
        {
            byte buffer = ReadRam(SecondsAddr);
            bool halted = (buffer & 0x80) > 0; //CH bit is set
            return !halted;
        }

        public DateTime GetDateTime()
        {
            byte[] buffer = ReadRam(SecondsAddr, 7);

            byte value = buffer[SecondsAddr];
            bool halted = (value & 0x80) > 0; //CH bit is set
            if (halted)
                throw new InvalidOperationException("DS1307 halted");

            byte second = value.FromBCD();
            byte minute = buffer[MinutesAddr].FromBCD();

            value = buffer[HoursAddr];
            bool mode12 = (value & 0x40) > 0; //12-hour mode
            if (mode12)
                throw new InvalidOperationException("DS1307 in 12-hour mode");

            byte hour = value.FromBCD();
            byte day = buffer[DateAddr].FromBCD();
            byte month = buffer[MonthAddr].FromBCD();
            int year = 2000 + buffer[YearAddr].FromBCD();

            var result = new DateTime(year, month, day, hour, minute, second);
            return result;
        }

        public void SetDateTime(DateTime dateTime)
        {
            var second = (byte) dateTime.Second;
            var minute = (byte) dateTime.Minute;
            var hour = (byte) dateTime.Hour;
            var dayOfWeek = (byte) (dateTime.DayOfWeek + 1);
            var day = (byte) dateTime.Day;
            var month = (byte) dateTime.Month;
            var year = (byte) (dateTime.Year - 2000);

            var buffer = new[]
                             {
                                 second.ToBCD(),
                                 minute.ToBCD(),
                                 hour.ToBCD(),
                                 dayOfWeek.ToBCD(),
                                 day.ToBCD(),
                                 month.ToBCD(),
                                 year.ToBCD()
                             };

            WriteRam(SecondsAddr, buffer);
        }

        public void EnableSquareWave(Rate rate)
        {
            var value = (byte)((byte)rate | 0x10);
            WriteRam(ControlAddr, value);
        }

        public void DisableSquareWave(bool sqwOutputState)
        {
            var value = (byte) (sqwOutputState ? 0x80 : 0x00);
            WriteRam(ControlAddr, value);
        }

        public void WriteRam(byte[] data)
        {
            if (data == null) 
                throw new ArgumentNullException("data");

            int len = data.Length;
            const int maxDataLen = RamLength - 3; // 2*Marker + length = 3
            if(len > maxDataLen) 
                throw new InvalidOperationException("Only " + maxDataLen + " bytes allowed");

            data = Utility.CombineArrays(new[] {OkMarker, OkMarker, (byte)data.Length}, data);
            WriteRam(RamAddr, data);
        }

        public byte[] ReadRam()
        {
            byte[] buffer = ReadRam(RamAddr, RamLength);
            if(buffer[0] != OkMarker || buffer[1] != OkMarker)
                return new byte[0];

            byte len = buffer[2];
            byte[] result = Utility.ExtractRangeFromArray(buffer, 3, len);
            return result;
        }

        private byte ReadRam(byte address)
        {
            byte[] result = ReadRam(address, 1);
            return result[0];
        }

        private byte[] ReadRam(byte address, int readBytes)
        {
            var result = new byte[readBytes];
            var transactions = new I2CDevice.I2CTransaction[2];

            transactions[0] = I2CDevice.CreateWriteTransaction(new[] {address});
            transactions[1] = I2CDevice.CreateReadTransaction(result);

            int actual = _device.Execute(transactions, Timeout);
            int expected = 1 + readBytes;

            if (actual != expected)
                throw new IOException("Unexpected I2C transaction result");

            return result;
        }

        private void WriteRam(byte address, params byte[] data)
        {
            if (data == null) 
                throw new ArgumentNullException("data");

            byte[] buffer = Utility.CombineArrays(new[] {address}, data);
            I2CDevice.I2CWriteTransaction transaction = I2CDevice.CreateWriteTransaction(buffer);
            var transactions = new I2CDevice.I2CTransaction[] {transaction};

            int actual = _device.Execute(transactions, Timeout);
            int expected = buffer.Length;

            if (actual != expected)
                throw new IOException("Unexpected I2C transaction result");
        }

        public void WriteRamString(string data)
        {
            byte[] buffer = Reflection.Serialize(data, typeof(string));
            WriteRam(buffer);
        }

        public string ReadRamString()
        {
            byte[] buffer = ReadRam();
            if(buffer.Length == 0)
                return String.Empty;

            var result = (string) Reflection.Deserialize(buffer, typeof(string));
            return result;
        }
    }
}
