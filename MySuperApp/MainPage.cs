using System.Collections.ObjectModel;

namespace MySuperApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext = new ViewModel();
        this
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(MainContent())
            .Padding(58);
    }

    CancellationTokenSource cts = new();
    Grid MainContent()
    {
        var grid = new Grid();

        grid.Margin(10).RowDefinitions("Auto, *")
            .Children(
                SearchView().Grid(row: 0, column: 0),
                Results().Grid(row: 1, column: 0)
            );

        return grid;

        TextBox SearchView()
        {
            var textBox = new TextBox().FontSize(10);

            textBox.TextChanged += (s, e) =>
            {
                var tb = (TextBox)s;
                if (tb.DataContext is not ViewModel vm)
                    return;

                if (cts is not null)
                {
                    cts.Cancel();
                }

                cts = new();

                Task.Delay(1000, cts.Token).ContinueWith(task =>
                {
                    if (task.IsFaulted && task.Exception != null)
                    {
                        throw task.Exception;
                    }

                    if (task.Status == TaskStatus.Canceled)
                    {
                        return;
                    }
                    _ = vm.DoSearch(tb.Text);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            };

            return textBox;
        }

        ListView Results()
        {
            var listView = new ListView();

            var source = ((ViewModel)DataContext).Items;

            listView.ItemTemplate(() =>
            {
                var txt = new TextBlock();
                txt.SetBinding(TextBlock.TextProperty, new Binding());
                return txt;
            })
            .ItemsSource(source);

            return listView;
        }
    }
}

class ViewModel
{
    public string Title { get; } = "My Super App";
    public ObservableCollection<string> Items { get; set; } = new() { "Hello", "World" };

    public async Task DoSearch(string value)
    {
        await Task.Delay(1000);
        Items.Add($"New Item {Items.Count - 2}");
    }
}
