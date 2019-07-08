using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class PODTimeEmptyException : Exception
    {
        public PODTimeEmptyException()
        {
        }

        public PODTimeEmptyException(string message)
          : base(message)
        {
        }

        public PODTimeEmptyException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected PODTimeEmptyException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}