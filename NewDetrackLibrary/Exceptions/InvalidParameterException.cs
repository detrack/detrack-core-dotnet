using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string message)
          : base(message)
        {
        }

        public InvalidParameterException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected InvalidParameterException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
