using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace DemoBlink1
{
    internal class LedRotator
    {
        int _currentIndex;
        private readonly OutputPort[] _leds;
        private readonly int _maxIndex;
        private bool _right = true;
        public bool Right
        {
            get { return _right; }
        }

        public LedRotator(params OutputPort[] leds)
        {
            if (leds == null) 
                throw new ArgumentNullException("leds");

            _maxIndex = leds.Length - 1;
            _leds = leds;
        }

        // ReSharper disable FunctionNeverReturns
        public void Run()
        {
            //krecimy diodamy
            for (;;)
            {
                _leds[_currentIndex].Write(true);
                Thread.Sleep(120);
                _leds[_currentIndex].Write(false);
                _currentIndex = GetNextIndex();
            }
        }
        // ReSharper restore FunctionNeverReturns

        private int GetNextIndex()
        {
            if(Right)
                return _currentIndex == _maxIndex ? 0 : _currentIndex + 1;

            return _currentIndex == 0 ? _maxIndex : _currentIndex - 1;
        }

        public void ChangeDirection()
        {
            _right = !_right;
        }
    }
}