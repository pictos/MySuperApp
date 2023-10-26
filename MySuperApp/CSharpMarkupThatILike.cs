using MGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace MySuperApp;

static class CSharpMarkupThatILike
{
    public static T Grid<T>(this T element, int row, int column)
        where T : UIElement
    {
        element.SetValue(MGrid.ColumnProperty, column);
        element.SetValue(MGrid.RowProperty, row);
        return element;
    }
}
