using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi.Framework
{

    public enum GraphTimeUnit
    {
        Unknown = 0,
        Month
    }

    public enum GraphAggregateType
    {
        Unknown = 0,
        HourOfDay,
        DayOfWeek
    }

    public interface ITextGraphData
    {
        DateTime Date { get; }
        int MessagesTotal { get; }
    }

    public interface ITextGraphDataCollection : ICollection<ITextGraphData>
    {

    }

    /// <summary>
    /// Inteface for producing graph data given parameters and an IConversation.
    /// </summary>
    public interface IGraphDataGenerator
    {
        ITextGraphDataCollection MessageCountPerUnitTime(IConversation conversation, GraphTimeUnit timeUnit);

        ITextGraphDataCollection MessageCountAggregate(IConversation conversation, GraphAggregateType aggregateType);
    }
}
