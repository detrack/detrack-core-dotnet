using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class EmptyDataException : Exception
    {
        public EmptyDataException()
        {
        }

        public EmptyDataException(string message)
          : base(message)
        {
        }

        public EmptyDataException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected EmptyDataException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}