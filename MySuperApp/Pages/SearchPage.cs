using System.Collections.ObjectModel;

namespace MySuperApp.Pages;


readonly record struct SearchResult(string Title, string ImageUrl, string Duration);
public partial class SearchPage : Page
{
    public SearchPage()
    {
        this.DataContext(new SearchViewModel(), (page, vm) =>
        {
            page
            .Background(Colors.White)
            .Content(MainContent(vm))
            .Padding(58);
        });
    }


    Grid MainContent(SearchViewModel vm)
    {
        return new Grid().RowDefinitions("50, *, auto")
            .Children
            (
                new TextBox().PlaceholderText("Search...")
                    .Grid(0, 0),
                new ListView()
                .ItemsSource(() => vm.Results)
                .Grid(1, 0)
                .ItemTemplate<SearchResult>
                (result =>
                    new Grid().Children
                    (
                        new Image().Source(() => result.ImageUrl).Grid(row: 0, rowSpan: 2).Stretch(Stretch.Uniform),
                        new StackPanel()
                        .Padding(5)
                        .Background("#99000000")
                        .Children
                        (
                            new TextBlock()
                            .Text(() => result.Title)
                            .Foreground(Colors.White)
                            .VerticalAlignment(VerticalAlignment.Center)
                            .TextWrapping(TextWrapping.WrapWholeWords),
                            new TextBlock().Foreground(Colors.White).Text(() => result.Duration)
                        ).Grid(1, 0)
                    ).RowDefinitions("* , auto")
                    .RowSpacing(6)
                ),
                new Button()
                .Content("Search")
                .Grid(2, 0)
                .MinWidth(100)
                .HorizontalAlignment(HorizontalAlignment.Center)
                .Assign(out var btn)
            );
    }
}

class SearchViewModel
{
    public ObservableCollection<SearchResult> Results { get; } = new ObservableCollection<SearchResult>()
    {
        new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30"),
        new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30"),
        new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30"),
    };


    public SearchViewModel()
    {
        Task.Delay(2000).ContinueWith(t =>
        {
            Results.Add(new SearchResult());
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

}
