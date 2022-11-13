namespace Client.ReNote
{
    internal class Client
    {
        public static Client Instance
        {
            get
            {
                return instance ??= new Client();
            }
            set
            {
                instance = value;
            }
        }
        public School SchoolInformation { get; set; }

        private static Client instance;
    }
}