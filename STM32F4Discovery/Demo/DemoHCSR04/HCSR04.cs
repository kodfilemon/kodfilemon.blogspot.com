using System;
using Common;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace DemoHCSR04
{
    // ReSharper disable InconsistentNaming
    public class HCSR04 : IDisposable // ReSharper restore InconsistentNaming
    {
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly object _syncRoot = new object();
        private bool _disposing;

        private static InterruptPort _echo;
        private static OutputPort _trigger;
        private long _startTime;
        private long _stopTime;

        public HCSR04(Cpu.Pin triggerPin, Cpu.Pin echoPin)
        {
            _echo = new InterruptPort(echoPin, false, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeBoth);
            _trigger = new OutputPort(triggerPin, false);

            _echo.OnInterrupt += (port, state, time) =>
                                     {
                                         if (state == 0)
                                         {
                                             _stopTime = time.Ticks;
                                             _resetEvent.Set();
                                             return;
                                         }

                                         _startTime = time.Ticks;
                                     };
        }

        public TimeSpan Ping()
        {
            lock (_syncRoot)
            {
                if (!_disposing)
                {
                    _trigger.Write(false);
                    _startTime = _stopTime = 0;

                    _resetEvent.Reset();
                    _trigger.Write(true);
                    Thread.Sleep(1);
                    _trigger.Write(false);
                    _resetEvent.WaitOne(60, false);

                    if (_startTime > 0 && _stopTime > 0)
                        return TimeSpan.FromTicks(_stopTime - _startTime);
                }
            }

            return TimeSpan.MaxValue;
        }

        public static float ToCentimeters(TimeSpan pulse)
        {
            if (pulse.Equals(TimeSpan.MaxValue))
                return Single.MaxValue;

            float result = pulse.TotalMicroseconds()/58f;
            return result;
        }

        public static float ToInches(TimeSpan pulse)
        {
            if (pulse.Equals(TimeSpan.MaxValue))
                return Single.MaxValue;

            float result = pulse.TotalMicroseconds()/148f;
            return result;
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                _disposing = true;

                _trigger.Dispose();
                _echo.Dispose();
            }
        }
    }
}
