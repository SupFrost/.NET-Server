using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Networking
{
    public class Global
    {
        public static DateTime PingTime { get; set; }
        public static ushort Ping { get; set; }
        public static Guid Guid { get; set; }

        public static Boolean Connected { get; set; }

    }
}
