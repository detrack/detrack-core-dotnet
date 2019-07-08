using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class DONumberNotFoundException : Exception
    {
        public DONumberNotFoundException()
        {
        }

        public DONumberNotFoundException(string message)
          : base(message)
        {
        }

        public DONumberNotFoundException(string message, Exception inner)
          : base(message, inner)
        {
        }

        protected DONumberNotFoundException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
