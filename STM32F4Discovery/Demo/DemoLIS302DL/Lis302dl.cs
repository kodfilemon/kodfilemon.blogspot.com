using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoLIS302DL
{
    public class Lis302Dl : IDisposable
    {
        private readonly SPI _spi;
        private double _currentSensitivity;

        private const byte WhoAmiReg = 0x0F;
        private const byte CtrlReg1 = 0x20;
        private const byte CtrlReg2 = 0x21;
        private const byte OutXReg = 0x29;

        public enum DataRate
        {
            F100Hz = 0,
            F400Hz = 1
        }

        public enum Scale
        {
            Full2K3,
            Full9K2
        }

        public Lis302Dl(SPI.SPI_module spiModule, Cpu.Pin chipSelect)
        {
            var spiCfg = new SPI.Configuration(chipSelect, false,
                                               0, 0, //5,8
                                               true, true,
                                               5000, spiModule);

            _spi = new SPI(spiCfg);

            byte[] whoAmI = Read(WhoAmiReg, 1);
            if (whoAmI[0] != 0x3B)
                throw new InvalidOperationException("LIS302DL not available");

            Write(CtrlReg1, 0x47);
            _currentSensitivity = ToSensitivity(Scale.Full2K3);
        }

        private double ToSensitivity(Scale scale)
        {
            return scale == Scale.Full2K3 ? 0.018 : 0.072;
        }

        public void Power(bool enable)
        {
            byte[] reg = Read(CtrlReg1, 1);
            reg[0] = enable ? Enable(reg[0], 0x40) : Disable(reg[0], 0x40);
            Write(CtrlReg1, reg[0]);

            if (enable)
                Thread.Sleep(30);
        }

        public void SetDataRate(DataRate value)
        {
            byte[] register = Read(CtrlReg1, 1);
            register[0] = value == DataRate.F100Hz
                              ? Disable(register[0], 0x80)
                              : Enable(register[0], 0x80);
            Write(CtrlReg1, register[0]);
        }

        public void SetScale(Scale value)
        {
            byte[] register = Read(CtrlReg1, 1);
            register[0] = value == Scale.Full2K3
                              ? Disable(register[0], 0x20)
                              : Enable(register[0], 0x20);
            Write(CtrlReg1, register[0]);
            _currentSensitivity = ToSensitivity(value);
        }

        public void EnableAxis(bool x, bool y, bool z)
        {
            byte[] register = Read(CtrlReg1, 1);

            byte newValue = register[0];
            newValue = x ? Enable(newValue, 0x01) : Disable(newValue, 0x01);
            newValue = y ? Enable(newValue, 0x02) : Disable(newValue, 0x02);
            newValue = z ? Enable(newValue, 0x04) : Disable(newValue, 0x04);
            register[0] = newValue;

            Write(CtrlReg1, register[0]);
        }

        public void Reboot()
        {
            byte[] register = Read(CtrlReg2, 1);
            register[0] = Enable(register[0], 0x40);
            Write(CtrlReg2, register[0]);
        }

        public void GetRaw(out sbyte x, out sbyte y, out sbyte z)
        {
            byte[] register = Read(OutXReg, 5);
            x = (sbyte)register[0];
            y = (sbyte)register[2];
            z = (sbyte)register[4];
        }

        public void GetAcc(out double x, out double y, out double z)
        {
            byte[] register = Read(OutXReg, 5);
            x = _currentSensitivity * (sbyte)register[0];
            y = _currentSensitivity * (sbyte)register[2];
            z = _currentSensitivity * (sbyte)register[4];
        }

        public void Dispose()
        {
            _spi.Dispose();
        }

        private byte[] Read(byte startAddress, byte count)
        {
            startAddress |= 0x80;
            if (count > 1)
                startAddress |= 0x40;

            var writeBuffer = new byte[1 + count];
            writeBuffer[0] = startAddress;

            var readBuffer = new byte[1 + count];
            _spi.WriteRead(writeBuffer, readBuffer);

            byte[] result = Utility.ExtractRangeFromArray(readBuffer, 1, count);
            return result;
        }

        private void Write(byte address, byte value)
        {
            byte[] writeBuffer = { address, value };
            _spi.Write(writeBuffer);
        }

        public static byte Enable(byte value, byte mask)
        {
            return (byte)(value | mask);
        }

        public static byte Disable(byte value, byte mask)
        {
            return (byte)(value & ~mask);
        }
    }
}
