using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uberminer
{
	public class NetworkWriter: BinaryWriter
    {
		public NetworkWriter(Stream stream) : base(stream) { }

		public override void Write(string str)
        {
			var length = Encoding.UTF8.GetByteCount(str);
			Write((byte)((length >> 8) & 0xFF));
			Write((byte)(length & 0xFF));
			Write(Encoding.UTF8.GetBytes(str));
		}

        public override void Write(short num)
        {
			Write((byte)((num >> 8) & 0xFF));
			Write((byte)(num & 0xFF));
		}

        public override void Write(Int32 num)
        {
			Write((byte)((num >> 24) & 0xFF));
			Write((byte)((num >> 16) & 0xFF));
			Write((byte)((num >> 8) & 0xFF));
			Write((byte)(num & 0xFF));
		}

        public override void Write(Int64 num)
        {
			//base.Write((Int64)EndianUtil.SwapBitShift(num));
			Write((byte)((num >> 56) & 0xFF));
			Write((byte)((num >> 48) & 0xFF));
			Write((byte)((num >> 40) & 0xFF));
			Write((byte)((num >> 32) & 0xFF));
			Write((byte)((num >> 24) & 0xFF));
			Write((byte)((num >> 16) & 0xFF));
			Write((byte)((num >> 8) & 0xFF));
			Write((byte)(num & 0xFF));
		}

        public override void Write(double val)
        {
			Write(BitConverter.DoubleToInt64Bits(val));
		}

		public override void Write(float val)
        {
			Write(EndianUtil.SingleToInt32Bits(val));
		}

        public void Write(short[] num)
        {
            foreach (var n in num)
            {
                Write((byte)((n >> 8) & 0xFF));
                Write((byte)(n & 0xFF));
            }
		}

        public void Write(Metadata val)
        {
        }

		public void WriteAngle(double val)
        {
			Write((byte)(val / 360.0 * 256));
		}
	}
}
