using Microsoft.UI;

namespace MySuperApp.Pages
{
    internal partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            this.DataContext(new PlayerViewModel(), (page, vm) =>
            {
                page
                .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
                .Content(MainContent())
                .Padding(58);
            });

            static Grid MainContent() => new Grid().DataContext<PlayerViewModel>((grid, vm) =>
            {
                grid.Children
                (
                    CircleImageGrid().Grid(1)
                );
            })
            .RowDefinitions("100,*,100");


            static Grid CircleImageGrid()
            {
                return new Grid().DataContext<PlayerViewModel>((grid, vm) =>
                {
                    grid.Children
                    (
                        CreateCircleImage(vm).Grid(),
                        new Button().Content("Play")
                        .Width(60).Height(60)
                        .HorizontalAlignment(HorizontalAlignment.Center)
                        .VerticalAlignment(VerticalAlignment.Center)
                        .Background("#DE5154")
                        .CornerRadius(30),
                        //.Command()
                        MusicInfo().Grid(1),
                        PlayerCommands().Grid(2)
                    );
                }).RowDefinitions("*, 48");


                static CircleImage CreateCircleImage(PlayerViewModel vm)
                {
                    var source = "https://avatars.githubusercontent.com/u/20712372?v=4";

                    var ci = new CircleImage().Margin(10).Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"));
                    ci.SetValue(CircleImage.SourceProperty, source);

                    return ci;
                }


                static Grid MusicInfo() => new Grid().DataContext<PlayerViewModel>((g, vm) =>
                {
                    g.Children
                    (
                        new TextBlock().VerticalAlignment(VerticalAlignment.Center).FontSize(16).Text(() => vm.CurrentTime).Grid(),
                        new Button().Background(Colors.Transparent).CommandParameter("REPEAT").FontSize(16).Grid(0, 3),
                        new TextBlock().VerticalAlignment(VerticalAlignment.Center).FontSize(16)
                    );
                }).ColumnDefinitions("Auto, *, Auto, Auto, *, Auto, 5");

                static StackPanel PlayerCommands() => new StackPanel().DataContext<PlayerViewModel>((s, vm) =>
                {
                    s.Children
                    (
                        new Button().CommandParameter("BACK"),
                        new Button().CommandParameter("BACKWARD"),
                        new Button().CommandParameter("FORWARD"),
                        new Button().CommandParameter("NEXT")
                    );
                })
                .Orientation(Orientation.Horizontal)
                .Spacing(5)
                .HorizontalAlignment(HorizontalAlignment.Center);
            }
        }
    }
}
