using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IOsVersion
    {
        int MajorVersion { get; }
        int MinorVersion { get; }
        int RevisionNumber { get; }
    }
}
