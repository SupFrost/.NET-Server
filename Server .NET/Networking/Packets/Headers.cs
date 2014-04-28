namespace Server.Networking.Packets
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
