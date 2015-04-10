using System;
using Microsoft.SPOT.Hardware;

namespace DemoIRReceiver
{
    public class IRReceiver : IDisposable
    {
        public delegate void PulseEventHandler(TimeSpan width, bool state);
        public event PulseEventHandler Pulse;

        private readonly InterruptPort _receiverPort;
        private long _lastTick = DateTime.Now.Ticks;

        public IRReceiver(Cpu.Pin pin)
        {
            _receiverPort = new InterruptPort(pin, false,
                                              Port.ResistorMode.PullUp,
                                              Port.InterruptMode.InterruptEdgeBoth);

            _receiverPort.OnInterrupt += PortInterrupt;
        }

        public void Dispose()
        {
            _receiverPort.Dispose();
        }

        private void PortInterrupt(uint port, uint state, DateTime time)
        {
            long current = time.Ticks;
            TimeSpan pulseWidth = TimeSpan.FromTicks(current - _lastTick);
            _lastTick = current;

            //Debug.Print("pulse " + pulseWidth.TotalMicroseconds()+ "us " + state);

            if (Pulse != null) 
                Pulse(pulseWidth, state == 1);
        }
    }
}
