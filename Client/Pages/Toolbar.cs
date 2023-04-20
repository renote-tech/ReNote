using System;
using System.Collections.Generic;

namespace Client.Pages
{
    public class Toolbar
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Type DefaultPage { get; set; }
        public Dictionary<string, Type> Buttons { get; set; }

        public Toolbar(string id, string name)
        {
            Id = id;
            Name = name;
            Buttons = new Dictionary<string, Type>();
        }
    }
}