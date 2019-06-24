using System;
namespace Exceptions
{
    public class DONumberNotFoundException : Exception
    {
        public DONumberNotFoundException() { }
        public DONumberNotFoundException(string message) : base(message) { }
        public DONumberNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected DONumberNotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
