using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Uberminer
{
    public class NetworkWriter : BinaryWriter
    {
        public StreamDirection direction;

        public NetworkWriter(Stream stream, StreamDirection dir)
            : base(stream)
        {
            direction = dir;
        }

        public override void Write(byte[] buffer)
        {
            base.Write(buffer, 0, buffer.Length);
        }

        public byte[] Prepare(string str)
        {
            var bytes = new byte[0];
            if (str != null)
            {
                MemoryStream m = new MemoryStream();
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    var length = str.Length;
                    writer.Write((byte)((length >> 8) & 0xFF));
                    writer.Write((byte)(length & 0xFF));

                    byte[] b1 = new byte[length * 2];
                    for (var i = 0; i < length * 2; i += 2)
                    {
                        byte[] b2 = BitConverter.GetBytes(str[i / 2]);
                        b1[i] = 0;
                        b1[i + 1] = b2[0];
                    }
                    writer.Write(b1);
                }
                bytes = m.ToArray();
            }

            return bytes;
        }

        public byte[] Prepare(byte val)
        {
            var bytes = new byte[1];
            bytes[0] = val;
            return bytes;
        }

        public byte[] Prepare(bool val)
        {
            var bytes = new byte[1];
            bytes = BitConverter.GetBytes(val);
            return bytes;
        }

        public byte[] Prepare(byte[] val)
        {
            return val;
        }

        public byte[] Prepare(short num)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write((byte)((num >> 8) & 0xFF));
                writer.Write((byte)(num & 0xFF));
            }
            var bytes = m.ToArray();
            return bytes;
        }

        public byte[] Prepare(Int32 num)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(num);
            }
            var bytes = m.ToArray();
            Array.Reverse(bytes);
            return bytes;
        }

        public byte[] Prepare(Int64 num)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write((byte)((num >> 56) & 0xFF));
                writer.Write((byte)((num >> 48) & 0xFF));
                writer.Write((byte)((num >> 40) & 0xFF));
                writer.Write((byte)((num >> 32) & 0xFF));
                writer.Write((byte)((num >> 24) & 0xFF));
                writer.Write((byte)((num >> 16) & 0xFF));
                writer.Write((byte)((num >> 8) & 0xFF));
                writer.Write((byte)(num & 0xFF));
            }
            var bytes = m.ToArray();
            return bytes;
        }

        public byte[] Prepare(double val)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(BitConverter.DoubleToInt64Bits(val));
            }
            var bytes = m.ToArray();
            Array.Reverse(bytes);
            return bytes;
        }

        public byte[] Prepare(float val)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(EndianUtil.SingleToInt32Bits(val));
            }
            var bytes = m.ToArray();
            return bytes;
        }

        public byte[] Prepare(short[] num)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                foreach (var n in num)
                {
                    writer.Write((byte)((n >> 8) & 0xFF));
                    writer.Write((byte)(n & 0xFF));
                }
            }
            var bytes = m.ToArray();
            return bytes;
        }

        public byte[] Prepare(Metadata val)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write(val.data);
            }
            var bytes = m.ToArray();
            return bytes;
        }

        public byte[] PrepareAngle(double val)
        {
            MemoryStream m = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(m))
            {
                writer.Write((byte)(val / 360.0 * 256));
            }
            var bytes = m.ToArray();
            return bytes;
        }
    }
}
