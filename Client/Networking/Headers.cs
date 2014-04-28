namespace Client.Networking
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
