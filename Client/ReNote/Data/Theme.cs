using Avalonia.Media;

namespace Client.ReNote.Data
{
    internal class Theme
    {
        public string Name { get; set; }
        public string ColorAccent { get; set; }
        public bool IsDarkTheme { get; set; }
        public int Id { get; set; }

        public Color Accent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ColorAccent))
                    return new Color();

                string[] rgb = ColorAccent.Split(',');
                if (rgb.Length != 3)
                    return new Color();

                return new Color(255, byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2]));
            }
        }

        public Color Primary
        {
            get
            {
                if (IsDarkTheme)
                    return new Color(255, 35, 35, 35);
                else
                    return new Color(255, 235, 235, 235);
            }
        }
    }
}