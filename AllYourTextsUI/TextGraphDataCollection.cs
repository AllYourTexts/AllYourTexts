using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    public class TextGraphData : ITextGraphData
    {
        public DateTime Date { get; set; }
        public int MessagesTotal { get; set; }

        public TextGraphData(DateTime date, int messaagesTotalCount)
        {
            Date = date;
            MessagesTotal = messaagesTotalCount;
        }

        public TextGraphData(DateTime date)
            :this(date, 0)
        {
            ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((TextGraphData)obj);
        }

        public bool Equals(TextGraphData other)
        {
            if (Date != other.Date)
            {
                return false;
            }

            if (MessagesTotal != other.MessagesTotal)
            {
                return false;
            }

            return true;
        }
    }

    public class TextGraphDataCollection : Collection<ITextGraphData>, ITextGraphDataCollection
    {
        public TextGraphDataCollection()
        {
            ;
        }
    }
}
