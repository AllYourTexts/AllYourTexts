using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    public class MockLoadingProgressCallback : ILoadingProgressCallback
    {
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        public virtual bool IsCancelRequested { get; protected set; }
        public bool IsEnded { get; private set; }
        public bool IsCanceled { get; protected set; }
        public List<LoadingPhase> PhaseHistory { get; private set; }
        public int CurrentPosition { get; private set; }
        public int LastIncrementedPosition { get; private set; }
        public LoadingPhase _currentPhase;

        private bool _hasBegun;

        public MockLoadingProgressCallback()
        {
            IsEnded = false;
            IsCanceled = false;
            PhaseHistory = new List<LoadingPhase>();
            LastIncrementedPosition = 0;
            _hasBegun = false;
        }

        public void Increment(int val)
        {
            EnsureHasBegun();

            CurrentPosition = Math.Min(CurrentPosition + val, Maximum);
            LastIncrementedPosition = CurrentPosition;
        }

        public void Begin(int maximum)
        {
            EnsureHasNotBegun();
            _hasBegun = true;

            Maximum = maximum;
        }

        public void End()
        {
            EnsureHasBegun();

            IsEnded = true;
            CurrentPosition = Maximum;
        }

        public void RequestCancel()
        {
            EnsureHasBegun();

            IsCancelRequested = true;
        }

        public void AcceptCancel()
        {
            EnsureHasBegun();

            IsEnded = true;
            IsCanceled = true;
        }

        public LoadingPhase CurrentPhase
        {
            get
            {
                return _currentPhase;
            }
            set
            {
                PhaseHistory.Add(value);
                _currentPhase = value;
            }
        }

        public void UpdateRemaining(int remaining)
        {
            Maximum = CurrentPosition + remaining;
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                EnsureHasBegun();

                throw new NotImplementedException();
            }
        }

        private void EnsureHasBegun()
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException("Begin() has not been called first.");
            }
        }

        private void EnsureHasNotBegun()
        {
            if (_hasBegun)
            {
                throw new InvalidOperationException("Begin() has already been called.");
            }
        }


        public void StartTimer()
        {
            throw new NotImplementedException();
        }
    }

    class MockLoadingProgressCallbackCancellable : MockLoadingProgressCallback
    {
        bool _IsCancelRequested;
        LoadingPhase _LoadingPhaseToCancel;

        public MockLoadingProgressCallbackCancellable(LoadingPhase loadingPhaseToCancel)
            :base()
        {
            _LoadingPhaseToCancel = loadingPhaseToCancel;
            _IsCancelRequested = false;
        }

        public override bool IsCancelRequested
        {
            get
            {
                if (CurrentPhase == _LoadingPhaseToCancel)
                {
                    RequestCancel();
                }
                return _IsCancelRequested;
            }
            protected set
            {
                _IsCancelRequested = value;
            }
        }
    }
}
