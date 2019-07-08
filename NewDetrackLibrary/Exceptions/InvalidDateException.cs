using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException()
        {
        }

        public InvalidDateException(string message)
          : base(message)
        {
        }

        public InvalidDateException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected InvalidDateException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
