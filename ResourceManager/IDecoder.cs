﻿using System.IO;
using Progression.Util;

namespace Progression.Resources.Manager
{
    public interface IDecoder<out T> : IDecoder
    {
        new T Decode(Stream stream);
    }
    public interface IDecoder : INameable
    {
        object Decode(Stream stream);
    }
}