using System;
using System.Threading;
using Microsoft.SPOT;

namespace DemoPWMSound
{
    public class RttlPlayer
    {
        private readonly ISpeaker _speaker;

        private static readonly double[][] Scales = new[]
                                                        {
                                                            new[]
                                                                {
                                                                    //C, Cis, D, Dis, E, F, Fis, G, Gis, A, Ais, H 
                                                                    261.6, 277.2, 293.7, 311.2, 329.2,
                                                                    349.6, 370, 391.9, 415.3, 440.0, 466.2, 493.9
                                                                },
                                                            new double[12],
                                                            new double[12],
                                                            new double[12]
                                                        };

        static RttlPlayer()
        {
            for (int i = 1; i < Scales.Length; i++)
                for (int j = 0; j < Scales[i].Length; j++)
                    Scales[i][j] = 2*Scales[i - 1][j];
        }

        public RttlPlayer(ISpeaker speaker)
        {
            _speaker = speaker;
        }

        public void Play(string rttlData)
        {
            const int oneBpmWholeNote = 60*4*1000; //one bpm whole note in ms

            Rttl rttl = Rttl.Parse(rttlData);
            int wholeNote = oneBpmWholeNote/rttl.Bpm;

            foreach (string tone in rttl.Tones)
            {
                bool specialDuration;
                string durationStr, noteStr, scaleStr;
                ParseCommand(tone, out durationStr, out noteStr, out scaleStr, out specialDuration);

                int duration = rttl.Duration;
                if (durationStr.Length > 0)
                    duration = Int32.Parse(durationStr);

                duration = specialDuration ? (3*wholeNote)/(2*duration) : wholeNote/duration;

                int freqIndex;
                switch (noteStr[0].ToLower())
                {
                    case 'c':
                        freqIndex = 0;
                        break;

                    case 'd':
                        freqIndex = 2;
                        break;

                    case 'e':
                        freqIndex = 4;
                        break;

                    case 'f':
                        freqIndex = 5;
                        break;

                    case 'g':
                        freqIndex = 7;
                        break;

                    case 'a':
                        freqIndex = 9;
                        break;

                    case 'b':
                        freqIndex = 11;
                        break;

                    default:
                        freqIndex = -1;
                        break;
                }

                if (noteStr.Length > 1) //#
                    freqIndex++;

                int scale = rttl.Octave;
                if (scaleStr.Length > 0)
                    scale = Int32.Parse(scaleStr);

                if (freqIndex >= 0)
                {
                    double freq = Scales[scale - 4][freqIndex];

                    Debug.Print("Playing: (" + tone + ")" + freq + " " + duration);
                    _speaker.Play(freq);
                    Thread.Sleep(duration);
                    _speaker.Pause();
                }
                else
                {
                    Debug.Print("Pausing: (" + tone + ") " + duration);
                    _speaker.Pause();
                    Thread.Sleep(duration);
                }
            }
        }

        private void ParseCommand(string tone, out string duration, out string note, out string scale,
                                  out bool specialDuration)
        {
            tone = tone.Trim();

            int len = tone.Length;
            int index = 0;

            while (IsDigit(tone[index]))
                index++;

            duration = tone.Substring(0, index);
            note = tone[index++].ToString();
            scale = String.Empty;
            specialDuration = false;

            if (index >= len)
                return;

            if (tone[index] == '#')
            {
                note += '#';
                index++;
            }

            if (index >= len)
                return;

            if (tone[index] == '.')
            {
                specialDuration = true;
                index++;
            }

            if (index >= len)
                return;

            scale = tone[index].ToString();
        }

        public static bool IsDigit(char value)
        {
            return value >= '0' && value <= '9';
        }
    }

    public class Rttl
    {
        public int Bpm { get; set; }
        public int Octave { get; set; }
        public int Duration { get; set; }
        public string[] Tones { get; set; }
        public string Name { get; set; }

        public static Rttl Parse(string rttlData)
        {
            string[] sections = rttlData.Split(':');

            string name = sections[0].Trim();
            string[] tones = sections[2].Split(',');
            int duration = 4;
            int octave = 6;
            int bpm = 63;

            if (sections[1].Length > 0)
            {
                string[] controls = sections[1].Split(',');
                foreach (string item in controls)
                {
                    string control = item.Trim();

                    string valueStr = control.Substring(2, control.Length - 2);
                    int value = Int32.Parse(valueStr);

                    switch (control[0].ToLower())
                    {
                        case 'd':
                            duration = value;
                            break;

                        case 'o':
                            octave = value;
                            break;

                        case 'b':
                            bpm = value;
                            break;
                    }
                }
            }

            var result = new Rttl
                             {
                                 Name = name,
                                 Tones = tones,
                                 Duration = duration,
                                 Octave = octave,
                                 Bpm = bpm
                             };
            return result;
        }
    }
}
