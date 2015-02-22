using System.ComponentModel;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    class LoadingProgressCallback : ProgressCallback, ILoadingProgressCallback
    {
        public LoadingPhase CurrentPhase { get; set; }

        public LoadingProgressCallback(BackgroundWorker backgroundWorker)
            : base(backgroundWorker)
        {
            ;
        }

        public void UpdateRemaining(int remaining)
        {
            int currentRemaining = _positionMaximum - CurrentPosition;

            if (currentRemaining != remaining)
            {
                _positionMaximum = CurrentPosition + remaining;
                ReportProgress();
            }
        }
    }
}
