using MGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace MySuperApp;

static class CSharpMarkupThatILike
{
    public static T Grid<T>(this T element, int row = 0, int column = 0)
        where T : UIElement
    {
        element.SetValue(MGrid.ColumnProperty, column);
        element.SetValue(MGrid.RowProperty, row);
        return element;
    }

    public static NavigationView MenuItems(this NavigationView view, params NavigationViewItem[] items)
    {
        foreach (var item in items)
            view.MenuItems.Add(item);

        return view;
    }
}
