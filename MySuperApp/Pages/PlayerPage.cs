
namespace MySuperApp.Pages
{
    internal partial class PlayerPage : Page
    {
        static SolidColorBrush backgroundColor = default!;
        public PlayerPage()
        {
            backgroundColor = (SolidColorBrush)Resources["ApplicationPageBackgroundThemeBrush"];

            this.DataContext(new PlayerViewModel(), (page, vm) =>
            {
                page
                .Background(backgroundColor)
                .Content(MainContent(vm))
                .Padding(58);
            });

            static Grid MainContent(PlayerViewModel vm) => new Grid().Children
                (
                    CircleImageGrid(vm).Grid(1)
                ).RowDefinitions("100,*,100");
        }

        static Grid CircleImageGrid(PlayerViewModel vm)
        {
            return new Grid().Children
                (
                    new CircleImage().Margin(10)
                    .Source(() => vm.Source)
                    .Background(backgroundColor)
                    .Grid(),
                    new Button().Content("Play")
                    .Width(60).Height(60)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .VerticalAlignment(VerticalAlignment.Center)
                    .Background("#DE5154")
                    .CornerRadius(30),
                    //.Command()
                    MusicInfo(vm).Grid(1),
                    PlayerCommands().Grid(2)
                ).RowDefinitions("*, 48");
        }

        static Grid MusicInfo(PlayerViewModel vm) => new Grid().Children
            (
                new TextBlock().VerticalAlignment(VerticalAlignment.Center).FontSize(16).Text(() => vm.CurrentTime).Grid(),
                new Button().Background(Colors.Transparent).CommandParameter("REPEAT").FontSize(16).Grid(0, 3),
                new TextBlock().VerticalAlignment(VerticalAlignment.Center).FontSize(16)
            ).ColumnDefinitions("Auto, *, Auto, Auto, *, Auto, 5");


        static StackPanel PlayerCommands() => new StackPanel().Children
            (
                new Button().CommandParameter("BACK"),
                new Button().CommandParameter("BACKWARD"),
                new Button().CommandParameter("FORWARD"),
                new Button().CommandParameter("NEXT")
            )
            .Orientation(Orientation.Horizontal)
            .Spacing(5)
            .HorizontalAlignment(HorizontalAlignment.Center);
    }
}

