using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class InvalidUpdateException : Exception
    {
        public InvalidUpdateException()
        {
        }

        public InvalidUpdateException(string message)
          : base(message)
        {
        }

        public InvalidUpdateException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected InvalidUpdateException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}