namespace Server.Resource.GUI
{
    internal enum ResourceType
    {
        UNKNOWN = 0,
        TEXT    = 1,
        IMAGE   = 2
    }

    internal enum ResourceAttribute
    {
        // Owner Attribute
        PUBLIC = 0x000,
        SHARED = 0x080,
        USER   = 0x100
    }
}