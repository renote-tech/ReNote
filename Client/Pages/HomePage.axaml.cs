namespace Client.Pages
{
    public partial class HomePage : Page
    {
        public static HomePage Instance { get; set; }

        public HomePage()
        {
            InitializeComponent();
            ToolBar = new ToolBar("Home");
            ToolBar.Buttons.Add("Edit Widgets", null);
        }
    }
}