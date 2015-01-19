using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class GraphDataGenerator : IGraphDataGenerator
    {
        public static readonly DateTime NormalizedSunday = new DateTime(2011, 1, 2, 0, 0, 0);

        public ITextGraphDataCollection MessageCountPerUnitTime(IConversation conversation, GraphTimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case GraphTimeUnit.Month:
                    return GetPerMonthDataCollection(conversation);
                default:
                    throw new ArgumentException("Invalid graph type", "timeUnit");
            }
        }

        public ITextGraphDataCollection MessageCountAggregate(IConversation conversation, GraphAggregateType aggregateType)
        {
            switch (aggregateType)
            {
                case GraphAggregateType.DayOfWeek:
                    return GetAggregateDayOfWeekDataCollection(conversation);
                case GraphAggregateType.HourOfDay:
                    return GetAggregateHourOfDayDataCollection(conversation);
                default:
                    throw new ArgumentException("Invalid graph type", "aggregateType");
            }
        }

        private static ITextGraphDataCollection GetPerMonthDataCollection(IConversation conversation)
        {
            TextGraphDataCollection dataCollection = new TextGraphDataCollection();

            if (conversation.MessageCount == 0)
            {
                return dataCollection;
            }

            Dictionary<DateTime, int> monthCounts = new Dictionary<DateTime, int>();

            foreach (IConversationMessage message in conversation)
            {
                DateTime messageTimestamp = message.Timestamp;
                DateTime firstDayOfMonth = new DateTime(messageTimestamp.Year, messageTimestamp.Month, 1);

                if (monthCounts.ContainsKey(firstDayOfMonth))
                {
                    monthCounts[firstDayOfMonth]++;
                }
                else
                {
                    monthCounts[firstDayOfMonth] = 1;
                }
            }

            foreach (DateTime date in monthCounts.Keys)
            {
                dataCollection.Add(new TextGraphData(date, monthCounts[date]));
            }

            return AdjustGraphDataCollectionForWorkaround(dataCollection);
        }

        /// <summary>
        /// Workaround for known issue with WPF Toolkit DateTimeAxis. If only one datapoint appears on a datetime, the
        /// graph renders no datapoints.
        /// http://wpf.codeplex.com/workitem/15276
        /// 
        /// Workaround: Add a datapoint immediately before and after the single datapoint.
        /// </summary>
        /// <param name="originalCollection"></param>
        /// <returns></returns>
        private static ITextGraphDataCollection AdjustGraphDataCollectionForWorkaround(TextGraphDataCollection originalCollection)
        {
            if (originalCollection.Count != 1)
            {
                return AddMonthlyZeroesWorkaround(originalCollection);
            }

            TextGraphData graphData = null;

            foreach (TextGraphData tgd in originalCollection)
            {
                graphData = tgd;
            }

            int month = graphData.Date.Month;
            DateTime prevDate;
            DateTime nextDate;

            if (month == 1)
            {
                prevDate = new DateTime(graphData.Date.Year - 1, 12, 1);
                nextDate = new DateTime(graphData.Date.Year, 2, 1);
            }
            else if (month == 12)
            {
                prevDate = new DateTime(graphData.Date.Year, 11, 1);
                nextDate = new DateTime(graphData.Date.Year + 1, 1, 1);
            }
            else
            {
                prevDate = new DateTime(graphData.Date.Year, month - 1, 1);
                nextDate = new DateTime(graphData.Date.Year, month + 1, 1);
            }

            TextGraphDataCollection adjustedCollection = new TextGraphDataCollection();

            adjustedCollection.Add(new TextGraphData(prevDate, 0));
            adjustedCollection.Add(graphData);
            adjustedCollection.Add(new TextGraphData(nextDate, 0));

            return adjustedCollection;
        }

        /// <summary>
        /// Workaround for an issue in the WPF toolkit where datapoint columns would be rendered too wide if only
        /// a few were present in the chart.
        /// </summary>
        /// <param name="graphDataCollection"></param>
        /// <returns></returns>
        private static ITextGraphDataCollection AddMonthlyZeroesWorkaround(ITextGraphDataCollection graphDataCollection)
        {
            List<ITextGraphData> sortedGraphDataCollection = new List<ITextGraphData>(graphDataCollection);
            sortedGraphDataCollection.Sort(delegate(ITextGraphData tgd1, ITextGraphData tgd2) { return tgd1.Date.CompareTo(tgd2.Date); });

            TextGraphDataCollection zeroAddedCollection = new TextGraphDataCollection();

            DateTime lastDate = DateTime.MinValue;

            foreach (ITextGraphData currentTextData in sortedGraphDataCollection)
            {
                if (lastDate != DateTime.MinValue)
                {
                    DateTime iteratorDate = lastDate.AddMonths(1);
                    while (iteratorDate < currentTextData.Date)
                    {
                        zeroAddedCollection.Add(new TextGraphData(iteratorDate));
                        iteratorDate = iteratorDate.AddMonths(1);
                    }
                }

                zeroAddedCollection.Add(currentTextData);
                lastDate = currentTextData.Date;
            }

            return zeroAddedCollection;
        }

        private static ITextGraphDataCollection GetAggregateDayOfWeekDataCollection(IConversation conversation)
        {
            const int daysPerWeek = 7;
            int[] messagesExchangedPerDayOfWeek = new int[daysPerWeek];

            for (int i = 0; i < daysPerWeek; i++)
            {
                messagesExchangedPerDayOfWeek[i] = 0;
            }

            foreach (IConversationMessage message in conversation)
            {
                messagesExchangedPerDayOfWeek[(int)message.Timestamp.DayOfWeek]++;
            }

            TextGraphDataCollection dataCollection = new TextGraphDataCollection();
            for (int i = 0; i < daysPerWeek; i++)
            {
                TextGraphData graphData = new TextGraphData(NormalizedSunday.AddDays(i));
                graphData.MessagesTotal = messagesExchangedPerDayOfWeek[i];
                dataCollection.Add(graphData);
            }

            return dataCollection;
        }

        private static ITextGraphDataCollection GetAggregateHourOfDayDataCollection(IConversation conversation)
        {
            const int hoursPerDay = 24;
            int[] messagesExchangedPerHour = new int[hoursPerDay];

            for (int i = 0; i < hoursPerDay; i++)
            {
                messagesExchangedPerHour[i] = 0;
            }

            foreach (IConversationMessage message in conversation)
            {
                messagesExchangedPerHour[message.Timestamp.Hour]++;
            }

            TextGraphDataCollection dataCollection = new TextGraphDataCollection();
            for (int i = 0; i < hoursPerDay; i++)
            {
                TextGraphData graphData = new TextGraphData(NormalizedSunday.AddHours(i));
                graphData.MessagesTotal = messagesExchangedPerHour[i];
                dataCollection.Add(graphData);
            }

            return dataCollection;
        }
    }
}
