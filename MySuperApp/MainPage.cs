using System.Collections.ObjectModel;

namespace MySuperApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext(new ViewModel(), (page, vm) =>
        {
            page
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(MainContent(vm))
            .Padding(58);
        });
    }

    CancellationTokenSource cts = new();

    Grid MainContent(ViewModel vm1)
    {

        return new Grid();
        //return new Grid().DataContext<ViewModel>((grid, vm) =>
        //{
        //    grid.Margin(10).RowDefinitions("Auto, *, Auto")
        //        .Children
        //        (
        //            SearchView(vm1).Grid(0, 0),
        //            Results(vm).Grid(1, 0),
        //            SearchButton(vm).Grid(2, 0).Assign(out var btn)
        //        ).VisualStateManager
        //        (b =>
        //            b.Group("ButtonState", gb =>
        //                gb.State("Entered", sb => sb.Setters(btn, e => e.Width(1500)))
        //                    .State("Exited", sb => sb.Setters(btn, e => e.Width(200))))
        //        );
        //});
    }

    TextBox SearchView(ViewModel vm)
    {
        var textBox = new TextBox().FontSize(10).Text(() => vm.SearchText);

        textBox.TextChanged += (s, e) =>
        {
            var tb = (TextBox)s;
            if (tb.DataContext is not ViewModel vm)
                return;

            cts?.Cancel();

            cts = new();

            Task.Delay(1000, cts.Token).ContinueWith(task =>
            {
                if (task is { IsFaulted: true, Exception: not null })
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

    static ListView Results(ViewModel vm)
    {
        var listView = new ListView();

        listView
        .ItemTemplate<string>((str) => new TextBlock().Text(() => str))
        .ItemsSource(() => vm.Items);

        return listView;
    }

    Button SearchButton(ViewModel vm)
    {
        var btn = new Button().Content("Find")
            .HorizontalAlignment(HorizontalAlignment.Center)
            .MinWidth(200);

        btn.Click += (s, e) =>
        {
            _ = vm.DoSearch(vm.SearchText);
        };

        btn.PointerEntered += (_, __) => VisualStateManager.GoToState(this, "Entered", true);

        btn.PointerExited += (_, __) => VisualStateManager.GoToState(this, "Exited", true);

        return btn;
    }
}


class ViewModel
{
    public string SearchText { get; set; } = string.Empty;
    public string Title { get; } = "My Super App";
    public ObservableCollection<string> Items { get; set; } = new() { "Hello", "World" };


    public async Task DoSearch(string value)
    {
        await Task.Delay(1000);
        Items.Add($"New Item {Items.Count - 2}");
    }
}
