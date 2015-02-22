using System;
using System.Collections.Generic;

namespace AllYourTextsLib.Framework
{
    public interface IContactList : IList<IContact>
    {
        void Sort(Comparison<IContact> comparer);
    }
}
