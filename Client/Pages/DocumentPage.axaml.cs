namespace Client.Pages
{
    public partial class DocumentPage : Page
    {
        public static DocumentPage Instance { get; set; }

        public DocumentPage()
        {
            InitializeComponent();
        }

        public override string GetToolbarId()
        {
            return "User";
        }
    }
}