using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Server.Networking.Packets
{
    class PacketWriter
    {
        private MemoryStream _ms;

        public PacketWriter()
        {
            _ms = new MemoryStream();
        }
 public byte[] GetBytes()
        {
            _ms.Close();
            byte[] data = _ms.ToArray();
            return data;
        }
        #region standard writes

        public void Write(int integer)
        {
            byte[] data = BitConverter.GetBytes(integer);
            _ms.Write(data, 0, data.Length);
        }

        public void Write(ushort value)
        {
            byte[] data = BitConverter.GetBytes(value);
            _ms.Write(data, 0, data.Length);
        }

        public void Write(byte[] data)
        {
            _ms.Write(data, 0, data.Length);
        }

        public void Write(Boolean value)
        {
            byte[] data = BitConverter.GetBytes(value);
            _ms.Write(data, 0, data.Length);
        }

        public void Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            _ms.Write(data, 0, data.Length);
        }


        #endregion

        #region non-standard writes
        public void Write(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ms.Close();

            byte[] imageBytes = ms.ToArray();
            Write(imageBytes.Length);
            Write(imageBytes);
        }
        public void Write(String value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value);

            Write(data.Length);
            Write(data);

        }
        public void Write(Guid guid)
        {
            byte[] data = guid.ToByteArray();

            Write(data.Length);
            Write(data);
        }
        #endregion


       
    }
    public class PacketReader
    {
        private MemoryStream _ms;
        public PacketReader(byte[] data)
        {
            _ms = new MemoryStream(data);
        }

        #region Standard Reads
        public int ReadInt32()
        {
            byte[] data = new byte[sizeof(Int32)];
            _ms.Read(data, 0, sizeof(Int32));

            return BitConverter.ToInt32(data, 0);
        }

        public ushort ReadUshort()
        {
            byte[] data = new byte[sizeof(ushort)];
            _ms.Read(data, 0, sizeof(ushort));

            return BitConverter.ToUInt16(data, 0);
        }

        public string ReadString()
        {
            int length = ReadInt32();
            var data = new byte[length];
            _ms.Read(data, 0,length);

            string value = Encoding.ASCII.GetString(data);

          return value;
        }

        public Boolean ReadBoolean()
        {
            byte[] data = new byte[sizeof(Boolean)];
            _ms.Read(data, 0, sizeof(Boolean));

            Boolean value = BitConverter.ToBoolean(data,0);
            return value;
        }

        public Double ReadDouble()
        {
            byte[] data = new byte[sizeof(Double)];
            _ms.Read(data, 0, sizeof(Double));

            Double value = BitConverter.ToDouble(data, 0);
            return value;
        }

        #endregion
        #region Non-standard Reads
 public byte[] ReadBytes(int length)
        {
            byte[] data = new byte[length];
            _ms.Read(data, 0, length);
            return data;
        }

        public Guid ReadGuid()
        {
            int length = ReadInt32();
            byte[] data = ReadBytes(length);

            Guid guid = new Guid(data);

            return guid;
        }

        public Image ReadImage()
        {
            int length = ReadInt32();
            byte[] bytes = ReadBytes(length);
            Image img;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                img = Image.FromStream(ms);
            }
            return img;
        }

        #endregion
     
    }

    }
