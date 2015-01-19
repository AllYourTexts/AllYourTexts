using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Framework
{
    public enum GraphType
    {
        Unknown = 0,
        AggregateHourOfDay,
        AggregateDayOfWeek,
        PerMonth
    }

    public interface IGraphWindowModel : IMainWindowModel
    {
        GraphType SelectedGraphType { get; set; }

        ITextGraphDataCollection CurrentGraphDataCollection { get; }
    }
}
