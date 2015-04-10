using System;
using System.IO;

namespace DemoLM15SGFNZ07Managed
{
    public class BinaryReader : IDisposable
    {
        private Stream _stream;

        public virtual Stream BaseStream
        {
            get { return _stream; }
        }

        public bool EndofStream
        {
            get { return _stream.Position == _stream.Length; }
        }

        public BinaryReader(Stream stream)
        {
            if (!stream.CanRead)
                throw new InvalidOperationException("Readonly stream");

            _stream = stream;
        }

        public virtual void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Stream stream = _stream;
            _stream = null;
            if (stream != null)
                stream.Close();
        }

        public int Peek()
        {
            if (!_stream.CanSeek)
                return -1;

            long position = _stream.Position;
            int num2 = Read();
            _stream.Position = position;
            return num2;
        }

        public virtual byte Read()
        {
            int num = _stream.ReadByte();
            if (num == -1)
                throw new IOException("End of stream");

            return (byte)num;
        }

        public virtual char ReadChar()
        {
            return (char)Read();
        }
    }
}