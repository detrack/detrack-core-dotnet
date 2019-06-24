using System;
namespace Exceptions
{
    public class EmptyDONumberException : Exception
    {
        public EmptyDONumberException() { }
        public EmptyDONumberException(string message) : base(message) { }
        public EmptyDONumberException(string message, System.Exception inner) : base(message, inner) { }
        protected EmptyDONumberException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
