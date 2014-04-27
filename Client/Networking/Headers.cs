namespace Client.Networking
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
