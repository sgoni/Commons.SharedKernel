using System;

namespace SharedKernel.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : ApplicationException
    {
        public ForbiddenAccessException() : base() { }
    }
}
