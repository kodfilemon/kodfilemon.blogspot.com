using System;
using System.IO.Ports;

namespace SerialController
{
    internal class Program
    {
        private static void Main()
        {
            using (var port = new SerialPort("COM18"))
            {
                port.DataReceived += DataReceived;
                port.Open();

                for (;;)
                {
                    string send = Console.ReadLine();
                    if (send.Length == 1 && send[0] == 'q')
                        return;

                    port.Write(send);
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

                bool printable = (b >= ' ' && b <= '~')
                                 || b == 0x0d
                                 || b == 0x0a;
                if (printable)
                    Console.Write(Convert.ToChar(b));
                else
                    Console.Write(b.ToString("X2"));
            }
        }
    }
}
