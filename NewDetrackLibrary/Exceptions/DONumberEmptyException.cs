// Decompiled with JetBrains decompiler
// Type: Exceptions.DONumberEmptyException
// Assembly: DetrackLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 698E8113-A420-4C5E-B6D9-F2AC4D4D34DC
// Assembly location: /Users/sesiliafeninagunawan/Desktop/Detrack/DetrackLibrary/bin/Debug/netstandard2.0/DetrackLibrary.dll

using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class DONumberEmptyException : Exception
    {
        public DONumberEmptyException()
        {
        }

        public DONumberEmptyException(string message)
          : base(message)
        {
        }

        public DONumberEmptyException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected DONumberEmptyException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
