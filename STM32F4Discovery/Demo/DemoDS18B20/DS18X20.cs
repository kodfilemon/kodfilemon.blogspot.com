using System;
using System.Collections;
using System.IO;
using Microsoft.SPOT.Hardware;

namespace DemoDS18B20
{
    // ReSharper disable InconsistentNaming
    public class DS18X20 // ReSharper restore InconsistentNaming
    {
        private readonly byte[] _presensePulse;
        private readonly OneWire _bus;
        private readonly bool _sSeries;

        // ReSharper disable InconsistentNaming
        private const byte DS18S20FamilyCode = 0x10;
        private const byte DS18B20FamilyCode = 0x28;

        private const byte MatchROMCommand = 0x55;
        private const byte ConvertTCommand = 0x44;
        private const byte ReadScratchpadCommand = 0xbe;
        private const int TimeoutMiliseconds = 750;
        // ReSharper restore InconsistentNaming

        public static DS18X20[] FindAll(OneWire bus)
        {
            if (bus == null)
                throw new ArgumentNullException("bus");

            var devices = new ArrayList();
            ArrayList presensePulses = bus.FindAllDevices();
            foreach (byte[] presensePulse in presensePulses)
            {
                if (presensePulse[0] != DS18S20FamilyCode
                    && presensePulse[0] != DS18B20FamilyCode)
                    continue;

                var device = new DS18X20(presensePulse, bus);
                devices.Add(device);
            }

            var result = (DS18X20[]) devices.ToArray(typeof (DS18X20));
            return result;
        }

        private DS18X20(byte[] presensePulse, OneWire bus)
        {
            if (presensePulse == null)
                throw new ArgumentNullException("presensePulse");

            if (bus == null)
                throw new ArgumentNullException("bus");

            if (presensePulse[0] != DS18S20FamilyCode
                && presensePulse[0] != DS18B20FamilyCode)
                throw new ArgumentException("Wrong pulses", "presensePulse");

            _presensePulse = presensePulse;
            _bus = bus;

            _sSeries = _presensePulse[0] == DS18S20FamilyCode;
        }

        private void RunSequence(byte command)
        {
            if (_bus.TouchReset() == 0) //0 = no devices, 1 = device(s) exist
                throw new IOException("DS18X20 communication error");

            Write(MatchROMCommand);
            Write(_presensePulse);
            Write(command);
        }

        public float GetTemperature()
        {
            RunSequence(ConvertTCommand);

            DateTime timeBarier = DateTime.Now.AddMilliseconds(TimeoutMiliseconds);
            while (_bus.ReadByte() == 0)
            {
                if (DateTime.Now > timeBarier)
                    throw new IOException("DS18X20 read timeout");
            }

            RunSequence(ReadScratchpadCommand);

            int reading = _bus.ReadByte() | (_bus.ReadByte() << 8); //lsb msb
            bool minus = (reading & 0x8000) > 0;
            if (minus)
                reading = (reading ^ 0xffff) + 1; //uzupelnienie do 2 (U2)

            float result = _sSeries
                               ? CalculateS(reading)
                               : CalculateB(reading);
            if (minus)
                result = -result;

            return result;
        }

        private static float CalculateB(int reading)
        {
            float result = 6*reading + reading/4; // multiply by (100 * 0.0625) or 6.25
            result = result/100;
            return result;
        }

        private static float CalculateS(int reading)
        {
            float result = (reading & 0x00FF)*0.5f;
            return result;
        }

        private void Write(params byte[] sendValues)
        {
            foreach (byte sendValue in sendValues)
                _bus.WriteByte(sendValue);
        }
    }
}