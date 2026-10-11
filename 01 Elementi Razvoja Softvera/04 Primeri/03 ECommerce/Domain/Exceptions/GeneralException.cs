using System;

namespace Domain.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException(string message) : base(message)
        {
        }
    }
}
