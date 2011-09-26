using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace uberminer
{
    public class NetworkReader : BinaryReader
    {
        public NetworkReader(Stream stream) : base(stream) { }

        public int Read(out byte b)
        {
            b = ReadByte();
            return 1;
        }

        public int Read(out short s)
        {
            s = IPAddress.NetworkToHostOrder(ReadInt16());
            return 2;
        }

        public int Read(out int i)
        {
            i = IPAddress.NetworkToHostOrder(ReadInt32());
            return 4;
        }

        public int Read(out long l)
        {
            l = IPAddress.NetworkToHostOrder(ReadInt64());
            return 8;
        }

        public int Read(out double d)
        {
            var a = ReadBytes(8);
            Array.Reverse(a);
            d = BitConverter.ToDouble(a, 0);
            return 8;
        }

        public int Read(out float f)
        {
            var a = ReadBytes(4);
            Array.Reverse(a);
            f = BitConverter.ToSingle(a, 0);
            return 4;
        }

        public int Read(out bool b)
        {
            b = ReadBoolean();
            return 1;
        }

        public int Read(out Metadata m)
        {
            /*
            let x = 0 of type UNSIGNED byte
            while (x = read byte from stream) does not equal 127:
                select based on value of (x >> 5):
                    case 0: read byte from stream
                    case 1: read short from stream
                    case 2: read int from stream
                    case 3: read float from stream
                    case 4: read string (UCS-2) from stream
                    case 5: read short, byte, short from stream; save as item stack (id, count, damage, respectively)
                    case 6: read int, int, int from stream; save as extra entity information.
                end select
            end while
             */

            int read = 0;

            var x = (byte)0x0;
            while (true)
            {
                Read(out x);

                ++read;

                if (x == 127)
                {
                    break;
                }
                switch (x >> 5)
                {
                    case 0x0:
                        {
                            byte b;
                            read += Read(out b);
                            break;
                        }
                    case 0x1:
                        {
                            short s;
                            read += Read(out s);
                            break;
                        }
                    case 0x2:
                        {
                            int i;
                            read += Read(out i);
                            break;
                        }
                    case 0x3:
                        {
                            float f;
                            read += Read(out f);
                            break;
                        }
                    case 0x4:
                        {
                            string s;
                            read += ReadS16(out s);
                            break;
                        }
                    case 0x5:
                        {
                            short s;
                            byte b;
                            read += Read(out s);
                            read += Read(out b);
                            read += Read(out s);
                            break;
                        }
                    case 0x6:
                        {
                            int i;
                            read += Read(out i);
                            read += Read(out i);
                            read += Read(out i);
                            break;
                        }
                }
            }

            m = new Metadata();
            return read;
        }

        public int Read(out WindowItemPayload p, int count)
        {
            /*
             offset = 0
 
             for slot in count:
                 item_id = payload[offset] as short
                 offset += 2
                 if item_id is not equal to -1:
                     count = payload[offset] as byte
                     offset += 1
                     uses = payload[offset] as short
                     offset += 2
                     inventory[slot] = new item(item_id, count, uses)
                 else:
                     inventory[slot] = None
                    }
             */
            var read = 0;

            for (int slot = 0; slot < count; ++slot)
            {
                short id;
                read += Read(out id);
                if (id != -1)
                {
                    byte quantity;
                    read += Read(out quantity);
                    short uses;
                    read += Read(out uses);
                }
            }

            p = new WindowItemPayload();
            return read;
        }

        public int Read(out byte[] b, int size)
        {
            int read = size;
            b = ReadBytes(size);
            return read;
        }

        public int Read(out short[] b, int size)
        {
            int read = size;
            var shorts = new short[size];
            for (var i = 0; i < size; ++i)
            {
                read += Read(out shorts[i]);
            }
            b = shorts;
            return read;
        }

        public int ReadAngle(out double angle)
        {
            var val = ReadByte();
            angle = val / 256.0 * 360.0;
            return 1;
        }

        public int ReadS16(out string s)
        {
            int read = 0;
            short length;
            read += Read(out length);

            string t = "";

            System.Diagnostics.Debug.Assert(length < 1000);

            if (length >= 0)
            {
                var enc = new UTF8Encoding();
                var b = ReadBytes(length * 2);

                for (var i = 0; i < length * 2; i += 2)
                {
                    t += (char)(b[i + 0] << 8 | b[i + 1]);
                }
                s = t;
                read += length * 2;
            }
            else
            {
                s = "";
            }

            return read;
        }

        // I don't think this type is ever used
        public int ReadS8(out string s)
        {
            int read = 0;
            short length;
            read += Read(out length);

            if (length >= 0)
            {
                s = Encoding.UTF8.GetString(ReadBytes(length));
                read += length;
            }
            else
            {
                s = "";
            }
            return read;
        }
    }
}
