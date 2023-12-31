namespace MySuperApp.Controls;

public partial class CircleImage
{
    static readonly PropertyChangedCallback changedCallback = new(OnPropertyChanged);
    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(string), typeof(CircleImage), new(string.Empty, changedCallback));

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    static async void OnPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs e)
    {
        if (bindable is not CircleImage circleImage || e.NewValue is null)
            return;

        await circleImage.GetImageAsync();
        circleImage?.Invalidate();
    }

    public static readonly new DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(SolidColorBrush), typeof(CircleImage), new(null, changedCallback));

    public new SolidColorBrush Background
    {
        get => (SolidColorBrush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
}
