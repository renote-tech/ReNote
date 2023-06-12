namespace Client.Exceptions;

using System;

public class UninitializedException : Exception
{
    public UninitializedException(string className, string functionName) : base(string.Format("Uninitialized class {0} at {1}", className, functionName)) { }
}