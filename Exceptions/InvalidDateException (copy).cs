using System;
namespace Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException() { }
        public InvalidDateException(string message) : base(message) { }
        public InvalidDateException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidDateException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
