﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public enum AttachmentType
    {
        Unknown = 0,
        Image,
        Video,
        Audio
    }

    public interface IMessageAttachment
    {
        AttachmentType Type { get; }

        string Path { get; }

        string OriginalFilename { get; }
    }
}
