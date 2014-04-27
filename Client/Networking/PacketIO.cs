using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Client.Networking
{
    class PacketWriter : BinaryWriter

    {
        private MemoryStream _ms;

        public PacketWriter() : base()
        {
            _ms = new MemoryStream();
            OutStream = _ms;
        }

        public byte[] GetBytes()
        {
            Close();
            byte[] data = _ms.ToArray();
            return data;
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

    public class PacketReader : BinaryReader
    {
        public PacketReader(byte[] data) : base(new MemoryStream(data))
        {



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
