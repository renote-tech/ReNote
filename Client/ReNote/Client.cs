namespace Client.ReNote
{
    internal class Client
    {
        public static Client Instance
        {
            get
            {
                return s_Instance ??= new Client();
            }
            set
            {
                s_Instance = value;
            }
        }

        public School SchoolInformation { get; set; }

        private static Client s_Instance;
    }
}