namespace AllYourTextsLib.Framework
{
    public interface IOsVersion
    {
        int MajorVersion { get; }
        int MinorVersion { get; }
        int RevisionNumber { get; }
    }
}
