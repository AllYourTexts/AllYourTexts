using System;
using System.ComponentModel;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class ProgressCallback : IProgressCallback
    {
        protected int _positionMaximum;

        private BackgroundWorker _backgroundWorker;

        private bool _hasBegun;

        public bool IsCanceled { get; private set; }
        
        public int CurrentPosition { get; protected set; }

        public ProgressCallback(BackgroundWorker backgroundWorker)
        {
            _backgroundWorker = backgroundWorker;
            IsCanceled = false;
            CurrentPosition = 0;

            _hasBegun = false;
        }

        public void Begin(int maximum)
        {
            if (_hasBegun)
            {
                throw new InvalidOperationException("Cannot begin ProgressCallback object more than once.");
            }
            else
            {
                _hasBegun = true;
            }

            CurrentPosition = 0;
            _positionMaximum = maximum;
        }

        public void Increment(int val)
        {
            CurrentPosition = Math.Min(CurrentPosition + val, _positionMaximum);

            //
            // Temper the progress reporting so as not to flood the UI thread with unnecessary update messages.
            //

            int reportingIncrements = Math.Max(_positionMaximum / 100, 1);
            if ((CurrentPosition % reportingIncrements) == 0)
            {
                ReportProgress();
            }
        }

        public void End()
        {
            CurrentPosition = _positionMaximum;

            ReportProgress();
        }

        public void RequestCancel()
        {
            _backgroundWorker.CancelAsync();
        }

        public bool IsCancelRequested
        {
            get
            {
                return _backgroundWorker.CancellationPending;
            }
        }

        public void AcceptCancel()
        {
            IsCanceled = true;
        }

        protected void ReportProgress()
        {
            int percentComplete = GetPercentComplete(CurrentPosition, _positionMaximum);

            _backgroundWorker.ReportProgress(percentComplete);
        }

        private static int GetPercentComplete(int amountCompleted, int amountTotal)
        {
            if (amountTotal == 0)
            {
                return 0;
            }

            double percentCompleteDouble = ((double)amountCompleted) / ((double)amountTotal) * 100;
            int percentComplete = (int)percentCompleteDouble;

            return percentComplete;
        }
    }
}
