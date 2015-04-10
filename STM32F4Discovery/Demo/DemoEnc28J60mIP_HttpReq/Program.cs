using Common;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Networking;

namespace DemoEnc28J60mIP_HttpReq
{
    public class Program
    {
        public static void Main()
        {
            const SPI.SPI_module spiBus = SPI.SPI_module.SPI3;
            const Cpu.Pin chipSelectPin = Stm32F4Discovery.FreePins.PA15;
            const Cpu.Pin interruptPin = Stm32F4Discovery.FreePins.PD1;
            const string hostname = "stm32f4";
            var mac = new byte[] {0x5c, 0x86, 0x4a, 0x00, 0x00, 0xdd};

            //Static IP
            //Adapter.IPAddress = "10.15.16.50".ToBytes();
            //Adapter.Gateway = "10.15.16.1".ToBytes();
            //Adapter.DomainNameServer = Adapter.Gateway;
            //Adapter.DomainNameServer2 = "8.8.8.8".ToBytes();  // Google DNS Server
            //Adapter.SubnetMask = "255.255.255.0".ToBytes();
            //Adapter.DhcpDisabled = true;

            //Adapter.VerboseDebugging = true;
            Adapter.Start(mac, hostname, spiBus, interruptPin, chipSelectPin);

            const int minVal = 0;
            const int maxVal = 100;

            string apiUrl = @"http://www.random.org/integers/?num=1"
                            + "&min=" + minVal + "&max=" + maxVal
                            + "&col=1&base=10&format=plain&rnd=new";

            var request = new HttpRequest(apiUrl);
            request.Headers.Add("Accept", "*/*");

            HttpResponse response = request.Send();
            if (response != null)
                Debug.Print("Random number: " + response.Message.Trim());
        }
    }
}
