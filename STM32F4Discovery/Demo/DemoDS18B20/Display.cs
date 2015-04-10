using System;

namespace DemoDS18B20
{
    public class Display : IDisplay
    {
        private readonly IDisplay[] _displays;

        public Display()
        {
            var debugDisplay = new DebugDisplay();
            var ledDisplay = new FourLedDisplay();
            var lcd = new LcdDisplay();
            _displays = new IDisplay[] {debugDisplay, ledDisplay, lcd};
        }

        public void ShowTemperature(float temperature)
        {
            foreach (IDisplay display in _displays)
                display.ShowTemperature(temperature);
        }

        public void ShowError(string message)
        {
            if (message == null) 
                throw new ArgumentNullException("message");

            foreach (IDisplay display in _displays)
                display.ShowError(message);
        }
    }
}