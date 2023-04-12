using Avalonia.Controls;
using System.Collections.Generic;

namespace Client.Pages
{
    public class Page : UserControl
    {
        public ToolBar ToolBar { get; set; }
    }

    public class ToolBar
    {
        public string Name { get; set; }
        public Dictionary<string, UserControl> Buttons { get; set; }

        public ToolBar(string name)
        {
            Name = name;
            Buttons = new Dictionary<string, UserControl>();
        }
    }
}