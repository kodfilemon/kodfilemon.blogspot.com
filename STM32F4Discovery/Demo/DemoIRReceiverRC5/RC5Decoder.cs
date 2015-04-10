using System;
using Common;
using Microsoft.SPOT;

namespace DemoIRReceiverRC5
{
    public class RC5Decoder
    {
        private const int Framelength = 14;
        private const int HalfBitTime = 889; //us
        private const int NextFrame = 3 * HalfBitTime; //3 * 889 us = 2667
        private const int MaxOneBitTime = HalfBitTime + HalfBitTime/5; //1.2*889 us = 1066

        public delegate void FrameDelegate(object sender, FrameEventArgs args);

        public class FrameEventArgs
        {
            public int Command { get; set; }
            public int Address { get; set; }
            public bool Toggle { get; set; }
        }

        public event FrameDelegate Frame;
        private readonly IRReceiver _receiver;
        private int _cnt;
        private bool _prevBit;
        int _frame;

        public RC5Decoder(IRReceiver receiver)
        {
            _receiver = receiver;
            _receiver.Pulse += ConsumePulse;
        }

        public static bool Check(params TimeSpan[] pulseTrain)
        {
            const int max = 1140; //max half bit time
            const int min = 640; //min half bit time

            foreach (TimeSpan pulse in pulseTrain)
            {
                var us = pulse.TotalMicroseconds();
                if(us > min && us <max)
                    return true;
            }

            return false;
        }

        private void ConsumePulse(TimeSpan width, bool state)
        {
            long usWidth = width.TotalMicroseconds();

            //poczatek ramki
            if (usWidth > NextFrame || _cnt == 0)
            {
                _cnt = 1;
                _prevBit = false; //zawsze zaczyna sie od 0
                _frame = 0;
                return;
            }

            if (_cnt == 0)
                return;

            //jesli szerokosc impulsu szersza niz 1 bit to dwa bity
            int bitCnt = usWidth > MaxOneBitTime ? 2 : 1;
            for (int i = 1; i <= bitCnt; i++)
            {
                _cnt++;

                if (_cnt%2 == 0)
                    DecodeMenchester(_prevBit, state); //co dwa bity dekodujemy bit ramki
                else
                {
                    //jesli juz mamy przedostatni bit to nie czekamy na ostatni mamy ca³¹ ramkê
                    if (_cnt == 27)
                    {
                        _cnt++;
                        //ostatni bit jest przeciwienstwem przedostatniego
                        //tutaj b³¹d w dekodowaniu menchester nie moze siê pojawiæ
                        DecodeMenchester(state, !state);
                        //mamy ramke
                        OnFrame(_frame);
                        //zaczynamy od nowa
                        _cnt = 0;
                    }
                    else
                        _prevBit = state; //to tylko kolejny bit
                }
            }
        }

        private void DecodeMenchester(bool bit0, bool bit1)
        {
            //kontrola czy jest ok
            if (!(bit0 ^ bit1))
            {
                Debug.Print("Invalid frame data");
                //jesli nie jest ok to zaczynamy ramke od nowa
                _cnt = 0;
            }

            //dekodowanie menchester: 01->1 , 10->0
            if (bit0) 
                return;

            _frame |= 1 << (Framelength - _cnt/2);
        }

        private void OnFrame(int frame)
        {
            int command = (frame & 0x3F);
            bool toggle = (frame & 0x0800) > 0;
            bool extended = (frame & 0x1000) == 0;
            int address = (frame & 0x1F) >> 6;
            //korekta dla extended RC5
            if (extended)
                command |= (1 << 6);

            //Debug.Print(" Addr:" + address + " Cmd:" + command + " Toggle: " + toggle);
            var args = new FrameEventArgs
                           {
                               Command = command,
                               Address = address,
                               Toggle = toggle
                           };

            if (Frame != null)
                Frame(this, args);
        }
    }
}