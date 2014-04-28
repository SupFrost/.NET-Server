using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Networking.Classes
{
    public enum IoHeader : ushort
    {
       Request,
       Send
    }

    public enum StandardHeader : ushort
    {
        Guid,
        Ping
    }
}
