using System;
using System.Runtime.Serialization;

namespace Comque.Exceptions
{
    [DataContract]
    public class InvalidOutputException : Exception
    {
        public InvalidOutputException() { }
        public InvalidOutputException(string message) : base(message) { }
        public InvalidOutputException(string message, Exception inner) : base(message, inner) { }
    }
}
