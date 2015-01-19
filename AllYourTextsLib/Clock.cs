using AllYourTextsLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
