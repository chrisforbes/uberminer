using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;

namespace uberminer
{
    public class UberStream : Stream
    {
        private BlockingCollection<byte> collection = new BlockingCollection<byte>();

        public UberStream()
        {
        }

        bool once = false;
        public override void Write(byte[] data, int offset, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                collection.Add(data[i]);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                buffer[offset + i] = collection.Take();
            }
            return count;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return collection.Count; }
        }




        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Flush()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool CanSeek
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
    }
}
