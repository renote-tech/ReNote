namespace Client.ReNote.Data;

using Avalonia.Media;

internal class Theme
{
    public string Name { get; set; }
    public string ColorAccent { get; set; }
    public bool IsDarkTheme { get; set; }
    public int Id { get; set; }

    private const byte PRIMARY_DARK = 35;
    private const byte PRIMARY_LIGHT = 235;

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
                return new Color(255, PRIMARY_DARK, PRIMARY_DARK, PRIMARY_DARK);
            else
                return new Color(255, PRIMARY_LIGHT, PRIMARY_LIGHT, PRIMARY_LIGHT);
        }
    }
}