using System;

namespace AllYourTextsLib.Framework
{
    public interface IPhoneNumber : IComparable
    {
        string Number { get; }
        string Country { get; }
    }
}
