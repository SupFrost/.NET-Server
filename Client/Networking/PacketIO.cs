using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Client.Networking
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

        public void Write(int integer)
        {
            byte[] data = BitConverter.GetBytes(integer);
            _ms.Write(data,0,data.Length);
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

        public void Write(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ms.Close();

            byte[] imageBytes = ms.ToArray();
            Write(imageBytes.Length);
            Write(imageBytes);
        }

        public void Write(Guid guid)
        {
            MemoryStream ms = new MemoryStream();
            byte[] data = guid.ToByteArray();
            ms.Write(data, 0, data.Length);

            ms.Close();

            byte[] buffer = ms.ToArray();
            Write(buffer.Length);
            Write(buffer);
            }


    }

    public class PacketReader 
    {
        private MemoryStream _ms;
        public PacketReader(byte[] data)
        {
            _ms = new MemoryStream(data);
            
        }

        public Int32 ReadInt32()
        {
            byte[] data = new byte[sizeof(Int32)];
            _ms.Read(data, 0, sizeof (Int32));

            return BitConverter.ToInt32(data, 0);
        }

        public ushort ReadUshort()
        {
            byte[] data = new byte[sizeof(ushort)];
            _ms.Read(data, 0, sizeof (ushort));

            return BitConverter.ToUInt16(data, 0);
        }

        public byte[] ReadBytes(int Length)
        {
            byte[] data = new byte[Length];
            _ms.Read(data, 0, Length);
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
    }


}
