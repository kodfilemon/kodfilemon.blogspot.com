using System;
using System.Threading;

namespace DemoHCSR04
{
    public class CollisionDetector : IDisposable
    {
        public delegate void StateChangedEventHandler(bool crash);

        public event StateChangedEventHandler StateChanged;

        public float Barier { get; set; }

        private readonly HCSR04 _sensor;
        private readonly Timer _timer;
        private bool _collision;

        public CollisionDetector(HCSR04 sensor, TimeSpan scanPeriod)
        {
            _sensor = sensor;
            _timer = new Timer(TimerTick, null, TimeSpan.Zero, scanPeriod);
        }

        private void TimerTick(object state)
        {
            TimeSpan pulse = _sensor.Ping();

            float distance = HCSR04.ToCentimeters(pulse);
            if (distance.Equals(Single.MaxValue))
                return;

            bool newState = distance <= Barier;
            if (_collision ^ newState)
            {
                _collision = newState;
                StateChangedEventHandler handler = StateChanged;
                if (handler != null)
                    StateChanged(_collision);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}