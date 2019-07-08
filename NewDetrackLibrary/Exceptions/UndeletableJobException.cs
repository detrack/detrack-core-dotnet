using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class UndeletableJobException : Exception
    {
        public UndeletableJobException()
        {
        }

        public UndeletableJobException(string message)
          : base(message)
        {
        }

        public UndeletableJobException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected UndeletableJobException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}