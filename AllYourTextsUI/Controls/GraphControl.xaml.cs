using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AllYourTextsUi.Framework;
using System.Windows.Controls.DataVisualization.Charting;
using Microsoft.Win32;
using System.IO;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        public GraphControl()
        {
            InitializeComponent();
        }

        public BitmapSource GetGraphBitmap()
        {
            return ElementToBitmap(textGraph);
        }

        public void UpdateToModel(IGraphWindowModel graphWindowModel)
        {
            SetGraphTitle(graphWindowModel);
            PopulateGraph(graphWindowModel);
            FormatAxes(graphWindowModel);
        }

        private void PopulateGraph(IGraphWindowModel graphWindowModel)
        {
            if (graphWindowModel.CurrentGraphDataCollection != null)
            {
                messagesTotalSeries.ItemsSource = graphWindowModel.CurrentGraphDataCollection;
            }
        }

        private void SetGraphTitle(IGraphWindowModel graphWindowModel)
        {
            string title;

            switch (graphWindowModel.SelectedGraphType)
            {
                case GraphType.AggregateHourOfDay:
                    title = "Texts Per Hour of Day";
                    break;
                case GraphType.AggregateDayOfWeek:
                    title = "Texts Per Day of Week";
                    break;
                case GraphType.PerMonth:
                    title = "Texts by Month";
                    break;
                default:
                    throw new ArgumentException();
            }

            IConversation selectedConversation = graphWindowModel.SelectedConversation;
            
            if (selectedConversation != null)
            {
                string contactNames = FormatAssociatedContactNamesForGraph(selectedConversation);
                textGraph.Title = string.Format("{0} - {1}", title, contactNames);
            }
            else
            {
                textGraph.Title = title;
            }
        }

        private static string FormatAssociatedContactNamesForGraph(IConversation conversation)
        {
            if (conversation.AssociatedContacts == null)
            {
                return "All Contacts";
            }

            if (conversation.AssociatedContacts.Count == 1)
            {
                return conversation.AssociatedContacts[0].DisplayName;
            }

            string[] contactNames = new string[conversation.AssociatedContacts.Count];
            for (int contactIndex = 0; contactIndex < conversation.AssociatedContacts.Count; contactIndex++)
            {
                contactNames[contactIndex] = conversation.AssociatedContacts[contactIndex].DisplayName;
            }

            return "Group Chat: " + string.Join(", ", contactNames);
        }

        private void FormatAxes(IGraphWindowModel graphWindowModel)
        {
            textGraphXAxis.Minimum = null;
            textGraphXAxis.Maximum = null;
            textGraphXAxis.IntervalType = DateTimeIntervalType.Auto;
            textGraphXAxis.Interval = null;

            switch (graphWindowModel.SelectedGraphType)
            {
                case GraphType.AggregateDayOfWeek:
                    TimeSpan dayPadding = TimeSpan.FromHours(12);
                    textGraphXAxis.Minimum = GraphDataGenerator.NormalizedSunday.Subtract(dayPadding);
                    textGraphXAxis.Maximum = GraphDataGenerator.NormalizedSunday.AddDays(7).Subtract(dayPadding);
                    textGraphXAxis.IntervalType = DateTimeIntervalType.Days;
                    textGraphXAxis.Title = "Day";
                    textGraphXAxis.Interval = 1.0;
                    textGraphXAxis.AxisLabelStyle = (Style)textGraph.FindResource("AggregatePerDayOfWeekAxisStyle");
                    break;
                case GraphType.AggregateHourOfDay:
                    TimeSpan hourPadding = TimeSpan.FromMinutes(30);
                    textGraphXAxis.Minimum = GraphDataGenerator.NormalizedSunday.Subtract(hourPadding);
                    textGraphXAxis.Maximum = GraphDataGenerator.NormalizedSunday.AddDays(1).Subtract(hourPadding);
                    textGraphXAxis.IntervalType = DateTimeIntervalType.Hours;
                    textGraphXAxis.Title = "Hour";
                    textGraphXAxis.Interval = 1.0;
                    textGraphXAxis.AxisLabelStyle = (Style)textGraph.FindResource("AggregatePerHourAxisStyle");
                    break;
                case GraphType.PerMonth:
                    DateTime minDate;
                    DateTime maxDate;
                    CalculateMonthAxisMinMax(graphWindowModel.CurrentGraphDataCollection, out minDate, out maxDate);
                    textGraphXAxis.IntervalType = DateTimeIntervalType.Months;
                    textGraphXAxis.Title = "Date";
                    textGraphXAxis.Interval = 1.0;
                    textGraphXAxis.Minimum = minDate;
                    textGraphXAxis.Maximum = maxDate;
                    textGraphXAxis.AxisLabelStyle = (Style)textGraph.FindResource("PerMonthAxisStyle");
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void CalculateMonthAxisMinMax(ITextGraphDataCollection graphDataCollection, out DateTime minDate, out DateTime maxDate)
        {
            DateTime min = DateTime.MaxValue;
            DateTime max = DateTime.MinValue;

            foreach (TextGraphData graphData in graphDataCollection)
            {
                if (graphData.Date < min)
                {
                    min = graphData.Date;
                }

                if (graphData.Date > max)
                {
                    max = graphData.Date;
                }
            }

            TimeSpan totalSpan = max.Subtract(min);

            //
            // Add padding if the total span is less than 180 days.
            //

            const int MinDaysWidth = 180;
            const double MinPadding = 15;
            double paddingAmount;

            if (totalSpan.TotalDays < MinDaysWidth)
            {
                paddingAmount = Math.Max((MinDaysWidth - totalSpan.TotalDays) / 2, MinPadding);
            }
            else
            {
                paddingAmount = MinPadding;
            }

            min = min.Subtract(TimeSpan.FromDays(paddingAmount));
            max = max.Add(TimeSpan.FromDays(paddingAmount));

            minDate = min;
            maxDate = max;
        }

        private static BitmapSource ElementToBitmap(FrameworkElement element)
        {
            double width = element.ActualWidth;
            double height = element.ActualHeight;

            RenderTargetBitmap elementBitmap = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }

            elementBitmap.Render(dv);

            return elementBitmap;
        }
    }
}
