using Server.Database.Utilities;

namespace Server.Database.Commands
{
    internal class Mnt
    {
        public static string Execute(string[] args)
        {
            if(!CommandUtil.IsExpectedLength(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            ReNote.Data.Database.Instance.SaveLocation = args[0];
            bool isLoaded = ReNote.Data.Database.Instance.Load();

            if (!isLoaded)
                return CommandMessages.DatabaseLoadFailed();

            return CommandMessages.Success();
        }
    }
}