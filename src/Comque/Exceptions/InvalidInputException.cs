using System;
using System.Runtime.Serialization;

namespace Comque.Exceptions
{
    [DataContract]
    public class InvalidInputException : Exception
    {
        public InvalidInputException() { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }
}
