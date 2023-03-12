using Server.Database.Utilities;
using ProtoBuf;
using Server.Common.Utilities;

namespace Server.Database.Commands
{
    internal class Sav
    {
        public static string Execute(string[] args)
        {
            if (!CommandUtil.IsExpectedLength(args, 1))
                return CommandMessages.InvalidParamsLength(1);

            if (ReNote.Data.Database.Instance.IsEmpty())
                return CommandMessages.DatabaseSaveFailed();

            if (FileUtil.IsFileBeingUsed(args[0]))
                return CommandMessages.DatabaseSaveFailed();

            using MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, ReNote.Data.Database.Instance);

            File.WriteAllBytes(args[0], stream.ToArray());
            return CommandMessages.Success();
        }
    }
}