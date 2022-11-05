namespace Server.Common.Exceptions
{
    public class ModuleFailedException : Exception
    {
        public ModuleFailedException() : base() { }

        public ModuleFailedException(string className, string functionName, string reason) :
            base(string.Format("Module failed: {0} at {1}\nReason: {2}", className, functionName, reason))
        { }
    }
}