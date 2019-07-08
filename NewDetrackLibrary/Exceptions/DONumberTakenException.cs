using System;
using System.Runtime.Serialization;

namespace Exceptions
 {
     public class DONumberTakenException : Exception
     {
         public DONumberTakenException()
         {
         }
 
         public DONumberTakenException(string message)
           : base(message)
         {
         }
 
         public DONumberTakenException(string message, Exception inner)
           : base(message, inner)
         {
         }
 
         protected DONumberTakenException(SerializationInfo info, StreamingContext context)
           : base(info, context)
         {
         }
     }
 }

