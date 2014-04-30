using System;

namespace Client.Networking
{
    public class Global
    {
        public static DateTime PingTime { get; set; }
        public static ushort Ping { get; set; }
        public static Guid Guid { get; set; }
        public static string Country { get; set; }
        public static Boolean Connected { get; set; }

    }
}
