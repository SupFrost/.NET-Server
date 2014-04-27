using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Networking.Classes
{
    public enum MainHeaders : ushort
    {
        Initial,
        Text
    }

    public enum InitialHeaders : ushort
    {
        Guid
    }
}
