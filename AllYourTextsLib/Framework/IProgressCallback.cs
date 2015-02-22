namespace AllYourTextsLib.Framework
{
    public interface IProgressCallback
    {
        void Begin(int maximum);

        void Increment(int val);

        void End();

        void RequestCancel();

        void AcceptCancel();

        bool IsCancelRequested { get; }

        bool IsCanceled { get; }

        int CurrentPosition { get; }
    }
}
