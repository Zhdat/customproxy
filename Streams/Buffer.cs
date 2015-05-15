using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Streams
{
    public class Buffer
    {
        List<byte> list;
        Encoding encoding;
        public Buffer()
        {
            this.list = new List<byte>();
            this.encoding = Encoding.UTF8;
        }
        public Buffer(string str)
        {
            Append(str);
        }
        public Buffer(byte[] bytes)
        {
            Append(bytes);
        }

        public void Append(byte b)
        {
            list.Add(b);
        }
        public void Append(byte[] bytes)
        {
            list.AddRange(bytes);
        }
        public void Append(string str)
        {
            Append(encoding.GetBytes(str));
        }
        public void Append(byte[] bytes, int offset, int count)
        {
            byte[] temp = new byte[count];
            Array.Copy(bytes, offset, temp, 0, count);
            list.AddRange(temp);
        }
        public void Clear()
        {
            list.Clear();
        }
        public int Count()
        {
            return list.Count;
        }

        internal byte[] GetBytes()
        {
            return list.ToArray();
        }

        internal int ReadMaximum(Stream stream)
        {
            const int ARRAY_SIZE = 1024;
            int read = 0;
            byte[] bufferIn = new byte[ARRAY_SIZE];
            do
            {
                read = stream.Read(bufferIn, 0, bufferIn.Length);
                Append(bufferIn, 0, read);
            } while (read == ARRAY_SIZE);
            return read;
        }
    }
}
