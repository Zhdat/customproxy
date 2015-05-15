using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Streams
{
    public class HeaderedStream : Stream
    {
        Stream innerStream;
        int header;
        bool headerRead;
        Buffer writeBuffer;
        Buffer readBuffer;
        public HeaderedStream(Stream inner)
        {
            this.innerStream = inner;
            this.writeBuffer = new Buffer();
            this.readBuffer = new Buffer();
        }
        public override bool CanRead
        {
            get { return innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return innerStream.CanWrite; }
        }

        public override void Flush()
        {
            if (writeBuffer.Count() > 0)
            {
                byte[] data = writeBuffer.GetBytes();
                innerStream.Write(data, 0, data.Length);
                writeBuffer.Clear();
            }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!this.CanRead)
                throw new InvalidOperationException();

            int read = readBuffer.ReadMaximum(this.innerStream);
            if (read == 0)
                return 0;


        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this.CanWrite)
                throw new InvalidOperationException();

            writeBuffer.Append(buffer, offset, count);
        }
    }
}
