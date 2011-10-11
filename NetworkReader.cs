using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Uberminer
{
    public class NetworkReader : BinaryReader
    {
        public StreamDirection direction;

        public NetworkReader(Stream stream, StreamDirection dir)
            : base(stream)
        {
            direction = dir;
        }

        public byte[] Read(out byte b)
        {
            var bytes = ReadBytes(1);
            b = bytes[0];
            return bytes;
        }

        public byte[] Read(out short s)
        {
            var bytes = ReadBytes(2);
            byte[] b = new byte[2];
            bytes.CopyTo(b, 0);
            Array.Reverse(b);
            s = BitConverter.ToInt16(b, 0);
            return bytes;
        }

        public byte[] Read(out int i)
        {
            var bytes = ReadBytes(4);
            byte[] b = new byte[4];
            bytes.CopyTo(b, 0);
            Array.Reverse(b);
            i = BitConverter.ToInt32(b, 0);
            return bytes;
        }

        public byte[] Read(out long l)
        {
            var bytes = ReadBytes(8);
            byte[] b = new byte[8];
            bytes.CopyTo(b, 0);
            Array.Reverse(b);
            l = BitConverter.ToInt64(b, 0);
            return bytes;
        }

        public byte[] Read(out double d)
        {
            var bytes = ReadBytes(8);
            byte[] b = new byte[8];
            bytes.CopyTo(b, 0);
            Array.Reverse(b);
            d = BitConverter.ToDouble(b, 0);
            return bytes;
        }

        public byte[] Read(out float f)
        {
            var bytes = ReadBytes(4);
            Array.Reverse(bytes);
            f = BitConverter.ToSingle(bytes, 0);
            return bytes;
        }

        public byte[] Read(out bool b)
        {
            var bytes = ReadBytes(1);
            b = BitConverter.ToBoolean(bytes, 0);
            return bytes;
        }

        public byte[] Read(out Metadata m)
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

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                while (true)
                {
                    var n = ReadBytes(1);
                    writer.Write(n);
                    var x = n[0];

                    if (x == 127)
                    {
                        break;
                    }
                    switch (x >> 5)
                    {
                        case 0x0:
                            {
                                // byte
                                writer.Write(ReadBytes(1));
                                break;
                            }
                        case 0x1:
                            {
                                // short
                                writer.Write(ReadBytes(2));
                                break;
                            }
                        case 0x2:
                            {
                                // int
                                writer.Write(ReadBytes(4));
                                break;
                            }
                        case 0x3:
                            {
                                // float
                                writer.Write(ReadBytes(2));
                                break;
                            }
                        case 0x4:
                            {
                                short s;

                                writer.Write(Read(out s));
                                writer.Write(ReadBytes(s * 2));
                                break;
                            }
                        case 0x5:
                            {
                                // short, byte, short
                                writer.Write(ReadBytes(5));
                                break;
                            }
                        case 0x6:
                            {
                                // int, int, int
                                writer.Write(ReadBytes(12));
                                break;
                            }
                    }
                }
            }

            m = new Metadata();
            m.data = ms.ToArray();
            return m.data;
        }

        public byte[] Read(out WindowItemPayload p, int count)
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

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                for (int slot = 0; slot < count; ++slot)
                {
                    short id;

                    writer.Write(Read(out id));

                    if (id != -1)
                    {
                        writer.Write(ReadBytes(3));
                    }
                }
            }

            p = new WindowItemPayload();
            p.data = ms.ToArray();
            return p.data;
        }

        public byte[] Read(out byte[] b, int size)
        {
            b = ReadBytes(size);
            return b;
        }

        public byte[] Read(out short[] b, int size)
        {
            var shorts = new short[size];

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                for (var i = 0; i < size; ++i)
                {
                    writer.Write(Read(out shorts[i]));
                }
            }

            b = shorts;
            return ms.ToArray();
        }

        public byte[] ReadAngle(out double angle)
        {
            var b = ReadBytes(1);
            angle = b[0] / 256.0 * 360.0;
            return b;
        }

        public byte[] Read(out string s)
        {
            int read = 0;
            short length;

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(Read(out length));

                string t = "";

                System.Diagnostics.Debug.Assert(length < 1000);

                if (length >= 0)
                {
                    var enc = new ASCIIEncoding();
                    var b = ReadBytes(length * 2);
                    writer.Write(b);

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
            }

            return  ms.ToArray();
        }

        public PacketType ReadPacketHeader()
        {
            return (PacketType)ReadByte();
        }

        // I don't think this type is ever used
        //public int ReadS8(out string s)
        //{
        //    int read = 0;
        //    short length;
        //    read += Read(out length);

        //    if (length >= 0)
        //    {
        //        s = Encoding.UTF8.GetString(ReadBytes(length));
        //        read += length;
        //    }
        //    else
        //    {
        //        s = "";
        //    }
        //    return read;
        //}
    }
}
