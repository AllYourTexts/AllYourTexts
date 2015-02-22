namespace AllYourTextsLib.Framework
{
    public enum LoadingPhase
    {
        Unknown = 0,
        Canceled,
        Error,
        ReadingContacts,
        ReadingChatInformation,
        ReadingMessages
    }

    public interface ILoadingProgressCallback : IProgressCallback
    {
        void UpdateRemaining(int remaining);

        LoadingPhase CurrentPhase { get; set; }
    }
}
