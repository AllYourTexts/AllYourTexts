using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IPhoneNumber : IComparable
    {
        string Number { get; }

        string Country { get; }
    }
}
