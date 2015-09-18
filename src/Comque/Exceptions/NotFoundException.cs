using System;
using System.Runtime.Serialization;

namespace Comque.Exceptions
{
    [DataContract]
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
