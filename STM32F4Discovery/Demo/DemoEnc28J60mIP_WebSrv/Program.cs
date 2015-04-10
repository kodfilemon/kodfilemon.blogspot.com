using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Text;
using Common;
using Microsoft.SPOT.Hardware;
using Networking;

namespace DemoEnc28J60mIP_WebSrv
{
    public class Program
    {
        private static Hashtable _leds;
        private static readonly object SyncRoot = new object();
        private const string Red = "Red";
        private const string Blue = "Blue";
        private const string Orange = "Orange";
        private const string Green = "Green";

        public static void Main()
        {
            Adapter.Start(new byte[] {0x5c, 0x86, 0x4a, 0x00, 0x00, 0xdd},
                          "stm32f4", SPI.SPI_module.SPI3,
                          Stm32F4Discovery.FreePins.PD1, Stm32F4Discovery.FreePins.PA15);

            Adapter.ListenToPort(80);
            Adapter.OnHttpReceivedPacketEvent += OnHttpReceivedPacketEvent;

            _leds = new Hashtable
                        {
                            {Red, new OutputPort(Stm32F4Discovery.LedPins.Red, false)},
                            {Blue, new OutputPort(Stm32F4Discovery.LedPins.Blue, false)},
                            {Orange, new OutputPort(Stm32F4Discovery.LedPins.Orange, false)},
                            {Green, new OutputPort(Stm32F4Discovery.LedPins.Green, false)}
                        };

            Thread.Sleep(Timeout.Infinite);
        }

        private static void OnHttpReceivedPacketEvent(HttpRequest request)
        {
            lock (SyncRoot)
            {
                int filePos = request.Path.LastIndexOf('/');
                string file = filePos == -1 ? String.Empty : request.Path.Substring(filePos + 1);

                int queryPos = file.LastIndexOf('?');
                string query = queryPos == -1 ? String.Empty : file.Substring(queryPos + 1);

                if (queryPos != -1)
                    file = file.Substring(0, queryPos);

                if (file == "led.svc")
                {
                    var led = _leds[query] as OutputPort;
                    if (led != null)
                        led.Write(!led.Read());

                    var sb = new StringBuilder("<html><head></head><body>");
                    foreach (string key in _leds.Keys)
                        sb.Append("<a href=\"/led.svc?" + key + "\">" + key + "</a> ");
                    sb.Append("</body></html>");

                    byte[] responseBuffer = Encoding.UTF8.GetBytes(sb.ToString());
                    using (var responseStream = new MemoryStream(responseBuffer))
                    {
                        request.SendResponse(new HttpResponse(responseStream));
                    }

                    return;
                }

                request.SendNotFound();
            }
        }
    }
}
