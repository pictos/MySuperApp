using System.Runtime.CompilerServices;
using MGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace MySuperApp;

static class Sugar
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Grid<T>(this T element, int row, int column)
        where T : UIElement =>
        element.Grid(column: column, columnSpan: null, row: row, rowSpan: null);

    public static NavigationView MenuItems(this NavigationView view, params NavigationViewItem[] items)
    {
        foreach (var item in items)
            view.MenuItems.Add(item);

        return view;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Vertical<T>(this T element)
        where T : StackPanel =>
        element.Orientation(Orientation.Vertical);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Horizontal<T>(this T element)
        where T : StackPanel =>
        element.Orientation(Orientation.Horizontal);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T HorizontalAlignmentCenter<T>(this T element)
        where T : FrameworkElement =>
        element.HorizontalAlignment(HorizontalAlignment.Center);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T HorizontalAlignmentLeft<T>(this T element)
        where T : FrameworkElement =>
        element.HorizontalAlignment(HorizontalAlignment.Left);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T HorizontalAlignmentRight<T>(this T element)
        where T : FrameworkElement =>
        element.HorizontalAlignment(HorizontalAlignment.Right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T HorizontalAlignmentStretch<T>(this T element)
        where T : FrameworkElement =>
        element.HorizontalAlignment(HorizontalAlignment.Stretch);
}
