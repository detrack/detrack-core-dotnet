using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class InvalidReattemptException : Exception
    {
        public InvalidReattemptException()
        {
        }

        public InvalidReattemptException(string message)
          : base(message)
        {
        }

        public InvalidReattemptException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected InvalidReattemptException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
