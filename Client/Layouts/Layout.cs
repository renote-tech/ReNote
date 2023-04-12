using Avalonia.Controls;

namespace Client.Layouts
{
    public class Layout : UserControl
    {
        public virtual void InitializeLanguage() 
        { }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeLanguage();
        }
    }
}