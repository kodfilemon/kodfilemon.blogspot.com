using System.Collections;
using System.IO;

namespace DemoSDCard2
{
    internal class SdCardRepository : IButtonEventsRepository
    {
        private readonly string _filePath;
        private readonly object _sync = new object();

        public SdCardRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(ButtonEvent buttonEvent)
        {
            string content = ButtonEventConverter.ToString(buttonEvent);

            lock(_sync)
            {
                using (TextWriter writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine(content);
                }
            }
        }

        public ButtonEvent[] GetAll()
        {
            var result = new ArrayList();

            if(File.Exists(_filePath))
            {
                lock (_sync)
                {
                    using (TextReader reader = new StreamReader(_filePath))
                    {
                        for (;;)
                        {
                            string line = reader.ReadLine();
                            if (line == null)
                                break;

                            ButtonEvent item = ButtonEventConverter.ToEntity(line);
                            result.Add(item);
                        }
                    }
                }
            }

            return (ButtonEvent[]) result.ToArray(typeof (ButtonEvent));
        }
    }
}
