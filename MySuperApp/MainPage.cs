namespace MySuperApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(new StackPanel()
            .Children(
                MainContent()
            ));
    }



    Grid MainContent()
    {
        var grid = new Grid();

        grid.Margin(10).RowDefinitions("Auto, *")
            .Children(
                SearchView().Grid(row: 0, column: 0),
                Results().Grid(row: 1, column: 0)
            );

        return grid;



        static TextBox SearchView()
        {
            var textBox = new TextBox().FontSize(10);

            textBox.TextChanged += (s, e) =>
            {

            };

            return textBox;
        }

        static ListView Results()
        {
            var listView = new ListView();

            listView.ItemTemplate(() =>
            {
                return new TextBlock().Text("Hello there!");
            })
                .ItemsSource(new[] { 1, 2, 3, 4 });

            return listView;
        }
    }
}
