using System.Collections;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace DemoBTHC05
{
    public class Program
    {
        public enum Command
        {
            Unknown,
            Red,
            Green,
            Blue,
            Orange
        }

        private static readonly Hashtable Led = new Hashtable();

        public static void Main()
        {
            Led.Add(Command.Red, new OutputPort(Stm32F4Discovery.LedPins.Red, false));
            Led.Add(Command.Green, new OutputPort(Stm32F4Discovery.LedPins.Green, false));
            Led.Add(Command.Blue, new OutputPort(Stm32F4Discovery.LedPins.Blue, false));
            Led.Add(Command.Orange, new OutputPort(Stm32F4Discovery.LedPins.Orange, false));

            using (var serial = new SerialPort("COM2"))
            {
                serial.Open();
                serial.DataReceived += DataReceived;

                byte[] buffer = Encoding.UTF8.GetBytes("Ping from STM32F4Discovery\r\n");
                for (;;)
                {
                    serial.Write(buffer, 0, buffer.Length);
                    Thread.Sleep(10000);
                }
            }
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort) sender;
            while (port.BytesToRead > 0)
            {
                int b = port.ReadByte();
                if (b == -1)
                    continue;

                Command command = ToCommand(b);
                if (command == Command.Unknown)
                {
                    Debug.Print(b.ToString("X2"));
                    continue;
                }

                var led = (OutputPort) Led[command];
                bool newValue = !led.Read();
                led.Write(newValue);

                string response = (char) b + "=" + (newValue ? "on" : "off") + "\r\n";
                byte[] buffer = Encoding.UTF8.GetBytes(response);
                port.Write(buffer, 0, buffer.Length);
            }
        }

        private static Command ToCommand(int value)
        {
            if (value == 'r')
                return Command.Red;

            if (value == 'g')
                return Command.Green;

            if (value == 'b')
                return Command.Blue;

            if (value == 'o')
                return Command.Orange;

            return Command.Unknown;
        }
    }
}
