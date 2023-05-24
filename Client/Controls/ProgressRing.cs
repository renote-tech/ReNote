namespace Client.Controls;

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;

internal class ProgressRing : TemplatedControl
{
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<ProgressRing, bool>(nameof(IsActive), defaultValue: true, notifying: OnActiveStateChanged);
    public static readonly DirectProperty<ProgressRing, double> MaxSideLengthProperty = AvaloniaProperty.RegisterDirect<ProgressRing, double>(nameof(MaxSideLength), o => o.MaxSideLength);
    public static readonly DirectProperty<ProgressRing, double> EllipseDiameterProperty = AvaloniaProperty.RegisterDirect<ProgressRing, double>(nameof(EllipseDiameter), o => o.EllipseDiameter);
    public static readonly DirectProperty<ProgressRing, Thickness> EllipseOffsetProperty = AvaloniaProperty.RegisterDirect<ProgressRing, Thickness>(nameof(EllipseOffset), o => o.EllipseOffset);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public double MaxSideLength
    {
        get { return m_MaxSideLength; }
        private set { SetAndRaise(MaxSideLengthProperty, ref m_MaxSideLength, value); }
    }

    public double EllipseDiameter
    {
        get { return m_EllipseDiameter; }
        private set { SetAndRaise(EllipseDiameterProperty, ref m_EllipseDiameter, value); }
    }

    public Thickness EllipseOffset
    {
        get { return m_EllipseOffset; }
        private set { SetAndRaise(EllipseOffsetProperty, ref m_EllipseOffset, value); }
    }

    private const string LARGE_STATE = ":large";
    private const string SMALL_STATE = ":small";

    private const string INACTIVE_STATE = ":inactive";
    private const string ACTIVE_STATE = ":active";

    private double m_MaxSideLength = 10;
    private double m_EllipseDiameter = 10;
    private Thickness m_EllipseOffset = new Thickness(2);

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        UpdateVisualStates();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        double maxSideLength = Math.Min(this.Width, this.Height);
        double ellipseDiameter = 0.1 * maxSideLength;
        if (maxSideLength <= 40)
            ellipseDiameter += 1;

        EllipseDiameter = ellipseDiameter;
        MaxSideLength = maxSideLength;
        EllipseOffset = new Thickness(0, maxSideLength / 2 - ellipseDiameter, 0, 0);

        UpdateVisualStates();
    }

    private static void OnActiveStateChanged(IAvaloniaObject sender, bool argument)
    {
        ((ProgressRing)sender).UpdateVisualStates();
    }

    private void UpdateVisualStates()
    {
        PseudoClasses.Remove(ACTIVE_STATE);
        PseudoClasses.Remove(INACTIVE_STATE);
        PseudoClasses.Remove(SMALL_STATE);
        PseudoClasses.Remove(LARGE_STATE);
        PseudoClasses.Add(IsActive ? ACTIVE_STATE : INACTIVE_STATE);
        PseudoClasses.Add(m_MaxSideLength < 60 ? SMALL_STATE : LARGE_STATE);
    }
}