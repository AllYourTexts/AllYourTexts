using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new
               UnhandledExceptionEventHandler(this.AppDomainUnhandledExceptionHandler);

            base.OnStartup(e);
        }

        void AppDomainUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs ea)
        {
            FailHandler.HandleUnrecoverableFailure((Exception)ea.ExceptionObject);
        }
    }
}
