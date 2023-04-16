using System;

namespace Server.Common.Exceptions
{
    public class UninitializedException : Exception
    {
        public UninitializedException() : base() 
        { }

        public UninitializedException(string className, string functionName) : base(string.Format("Uninitialized class {0} at {1}", className, functionName))
        { }
    }
}