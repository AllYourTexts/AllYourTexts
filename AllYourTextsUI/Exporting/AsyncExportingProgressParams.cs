using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using AllYourTextsUi.Models;

namespace AllYourTextsUi
{
    public class AsyncExportingProgressParams
    {
        public IProgressCallback ProgressCallback { get; private set; }

        public ExportMultipleDialogModel Model { get; private set; }

        public ExportMultipleDialogModel.ExportFormat ExportFormat { get; private set; }

        public string ExportPath { get; private set; }

        public AsyncExportingProgressParams(IProgressCallback progressCallback, ExportMultipleDialogModel model, ExportMultipleDialogModel.ExportFormat exportFormat, string exportPath)
        {
            ProgressCallback = progressCallback;

            Model = model;

            ExportFormat = exportFormat;

            ExportPath = exportPath;
        }
    }
}
