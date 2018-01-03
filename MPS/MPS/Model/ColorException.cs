using System;

namespace MPS.Model
{
    public class ColorException : Exception
    {
        public ColorException()
        {
        }

        public ColorException(string message) : base(message)
        {
        }

        public ColorException(string message, Exception innerException) : base(message, innerException)
        {
        }


    }
}