namespace Client.Pages;

public partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
    }

    public override string GetToolbarId()
    {
        return "Home";
    }
}