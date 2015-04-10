using System;

namespace DemoSDCard2
{
    internal class ButtonEvent
    {
        public ButtonEvent(DateTime eventTime, bool state)
        {
            EventTime = eventTime;
            State = state;
        }

        public DateTime EventTime { get; set; }
        public bool State { get; set; }
    }
}