using Avalonia.Controls;

namespace Client.Pages
{
    public class Page : UserControl
    {
        public virtual string GetToolbarId()
        {
            return null;
        }
    }
}