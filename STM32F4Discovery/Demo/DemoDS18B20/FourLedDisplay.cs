using System;
using System.Threading;
using Common;
using Microsoft.SPOT.Hardware;

namespace DemoDS18B20
{
    public class FourLedDisplay : IDisplay, IDisposable
    {
        private const int BlinkPeriod = 1000;

        private readonly LedRange[] _ranges;
        private Timer _timer;
        
        private sealed class LedRange
        {
            public OutputPort Led { get; private set; }
            private float Range { get; set; }

            public LedRange(OutputPort led, float range)
            {
                Led = led;
                Range = range;
            }

            public void Check(float temperature)
            {
                bool newState = temperature >= Range;
                Led.Write(newState);
            }
        }

        public FourLedDisplay()
        {
            var blueLed = new OutputPort(Stm32F4Discovery.LedPins.Blue, false);
            var greenLed = new OutputPort(Stm32F4Discovery.LedPins.Green, false);
            var orangeLed = new OutputPort(Stm32F4Discovery.LedPins.Orange, false);
            var redLed = new OutputPort(Stm32F4Discovery.LedPins.Red, false);

            _ranges = new[]
                          {
                              new LedRange(blueLed, 20),
                              new LedRange(greenLed, 25),
                              new LedRange(orangeLed, 30),
                              new LedRange(redLed, 35)
                          };
        }

        public void ShowTemperature(float temperature)
        {
            if(_timer != null)
                _timer.Change(Timeout.Infinite, BlinkPeriod);

            foreach (LedRange range in _ranges)
                range.Check(temperature);
        }

        public void ShowError(string message)
        {
            if(_timer == null)
            {
                TimerCallback blinkAction = state =>
                                                {
                                                    bool ledState = _ranges[0].Led.Read();
                                                    foreach (LedRange range in _ranges)
                                                        range.Led.Write(!ledState);
                                                };

                _timer = new Timer(blinkAction, null, 0, BlinkPeriod);
            }
            else
                _timer.Change(0, BlinkPeriod);

        }

        public void Dispose()
        {
            if(_timer != null)
                _timer.Dispose();
        }
    }
}