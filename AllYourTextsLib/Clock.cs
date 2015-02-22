using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class Clock : IClock
    {
        public DateTime CurrentTime()
        {
            return DateTime.Now;
        }
    }
}
