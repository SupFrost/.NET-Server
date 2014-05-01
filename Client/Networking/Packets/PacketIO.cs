using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Client.Networking.Packets
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
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ms.Close();

            var imageBytes = ms.ToArray();
            Write(imageBytes.Length);
            Write(imageBytes);
        }
        public void Write(String value)
        {
            var data = Encoding.ASCII.GetBytes(value);

            Write(data.Length);
            Write(data);

        }
        public void Write(Guid guid)
        {
            var data = guid.ToByteArray();

            Write(data.Length);
            Write(data);
        }
        #endregion



    }
    public class PacketReader
    {
        private readonly MemoryStream _ms;
        public PacketReader(byte[] data)
        {
            _ms = new MemoryStream(data);
        }

        #region Standard Reads
        public Int32 ReadInt32()
        {
            var data = new byte[sizeof(Int32)];
            _ms.Read(data, 0, sizeof(Int32));

            return BitConverter.ToInt32(data, 0);
        }

        public ushort ReadUshort()
        {
            var data = new byte[sizeof(ushort)];
            _ms.Read(data, 0, sizeof(ushort));

            return BitConverter.ToUInt16(data, 0);
        }

        public string ReadString(int length)
        {
            var data = new byte[length];
            _ms.Read(data, 0, length);

            var value = Encoding.ASCII.GetString(data);


            return value;
        }

        #endregion
        #region Non-standard Reads
        public byte[] ReadBytes(int length)
        {
            var data = new byte[length];
            _ms.Read(data, 0, length);
            return data;
        }

        public Guid ReadGuid()
        {
            var length = ReadInt32();
            var data = ReadBytes(length);

            var guid = new Guid(data);

            return guid;
        }

        public Image ReadImage()
        {
            int length = ReadInt32();
            byte[] bytes = ReadBytes(length);
            Image img;
            using (var ms = new MemoryStream(bytes))
            {
                img = Image.FromStream(ms);
            }
            return img;
        }

        #endregion

    }
}
