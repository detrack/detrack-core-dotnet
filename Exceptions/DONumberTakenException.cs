using System;

namespace Exceptions
{
    public class DONumberTakenException : Exception
    {
        public DONumberTakenException() { }
        public DONumberTakenException(string message) : base(message) { }
        public DONumberTakenException(string message, System.Exception inner) : base(message, inner) { }
        protected DONumberTakenException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
