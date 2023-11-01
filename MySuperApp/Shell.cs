using System.Collections.ObjectModel;
using static MySuperApp.AppPages;


namespace MySuperApp;

partial class Shell : Page
{
    readonly Frame contentFrame;

    public Shell()
    {
        new NavigationView()
            .Assign(out var navigationView)
            .IsSettingsVisible(false)
            .IsBackButtonVisible(NavigationViewBackButtonVisible.Collapsed)
            .PaneDisplayMode(NavigationViewPaneDisplayMode.LeftMinimal)
            .MenuItems
            (
                new NavigationViewItem().Content(Player),
                new NavigationViewItem().Content(Search)
            )
            .Content
            (
                new Grid().Children
                (
                    new Frame().Assign(out contentFrame).Grid(1)
                )
                .RowDefinitions("48, *")
            )
            .SelectionChanged += OnNavigationViewSelectionChanged;

        navigationView
            .SelectedItem(navigationView.MenuItems[0]);

        this.Content(navigationView);
    }

    static readonly ReadOnlyDictionary<AppPages, Page> pages = new Dictionary<AppPages, Page>()
    {
        { Player, new PlayerPage() },
        { Search, new MainPage() }
    }.AsReadOnly();

    void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var item = (NavigationViewItem)args.SelectedItem;

        switch ((AppPages)item.Content)
        {
            case Search:
                contentFrame.Content = pages[Search];
                break;
            case Player:
                contentFrame.Content = pages[Player];
                break;
        }
    }
}

enum AppPages
{
    Player,
    Search
}
