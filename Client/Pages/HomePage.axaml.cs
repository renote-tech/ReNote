namespace Client.Pages
{
    public partial class HomePage : Page
    {
        public static HomePage Instance { get; set; }

        public HomePage()
        {
            InitializeComponent();
        }

        public override string GetToolbarId()
        {
            return "Home";
        }
    }
}