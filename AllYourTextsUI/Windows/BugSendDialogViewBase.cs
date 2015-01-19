using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AllYourTextsLib.Framework;
using System.ComponentModel;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    public abstract class BugSendDialogViewBase : Window
    {
        protected IBugSendDialogModel _model;

        private BackgroundWorker _backgroundWorker;

        public BugSendDialogViewBase()
            : this(null)
        {
            ;
        }

        public BugSendDialogViewBase(IBugSendDialogModel model)
        {
            _model = model;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = false;
            _backgroundWorker.WorkerSupportsCancellation = false;
            _backgroundWorker.DoWork += OnSendStart;
            _backgroundWorker.RunWorkerCompleted += OnSendCompleted;
        }

        protected abstract void ViewToModel();

        protected abstract void ModelToView();

        protected abstract void DisableSendControls();

        protected abstract void EnableSendControls();

        protected abstract void ShowProgressBar();

        protected abstract void HideProgressBar();

        protected abstract void ShowError();

        protected abstract void HideError();

        protected void UploadInformation()
        {
            ViewToModel();

            if (!_model.CanUploadInformation)
            {
                ShowError();
                return;
            }

            DisableSendControls();

            ShowProgressBar();
            HideError();

            _backgroundWorker.RunWorkerAsync();
        }

        private void OnSendStart(object sender, DoWorkEventArgs e)
        {
            _model.UploadInformation();

            e.Result = _model.SendInformationSucceeded;
        }

        private void OnSendCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableSendControls();

            HideProgressBar();

            if (_model.SendInformationSucceeded)
            {
                DialogResult = true;
            }
            else
            {
                ShowError();
            }
        }
    }
}
