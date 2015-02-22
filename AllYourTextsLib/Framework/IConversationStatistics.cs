namespace AllYourTextsLib.Framework
{
    public interface IConversationStatistics
    {
        int MessagesSent { get; }
        int MessagesReceived { get; }
        int DayCount { get; }
        int MessagesExchanged { get; }
        double MessagesPerDay { get; }
    }
}
