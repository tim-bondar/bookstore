using System;

namespace Core.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(string message) : base(message)
        {
        }
    }
}
