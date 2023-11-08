using Microsoft.UI.Xaml.Media.Animation;

namespace MySuperApp.Pages;

internal partial class PlayerPage : Page
{
	static SolidColorBrush backgroundColor = default!;
	CircleImage circleImage = default!;
	readonly Storyboard storyboard;
	
	static readonly Style<Button> buttonStyle = new Style<Button>()
		.Setters(s =>
			s.MinWidth(50).MinHeight(50).Foreground(Colors.Black).Background(Colors.Transparent).BorderThickness(0));

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
		storyboard = RotateAnimation();

		this.Resources(r => r.Add("rotateImg",   storyboard).Add(buttonStyle));
		//storyboard.Begin();

		Grid MainContent(PlayerViewModel vm) => new Grid().Children
		(
			CircleImageGrid(vm).Grid(1,0)
		).RowDefinitions("100,*,100")
			.Resources(r => r.Add(buttonStyle));


		Storyboard RotateAnimation()
		{
			var storyboard = new Storyboard().Children
				(
					new DoubleAnimation().Assign(out var animation)
					.From(0)
					.To(360)
					.Duration(new Duration(TimeSpan.FromSeconds(5)))
					.RepeatBehavior(RepeatBehavior.Forever)
				);

			Storyboard.SetTarget(animation, circleImage);
			Storyboard.SetTargetProperty(animation, "(UIElement.RenderTransform).(RotateTransform.Angle)");
			return storyboard;
		}
	}


	Grid CircleImageGrid(PlayerViewModel vm)
	{
		
		var grid = new Grid().Children
			(
				new CircleImage().Margin(10)
				.Name("circleImg")
				.Assign(out circleImage)
				.Source(() => vm.Source)
				.Background(backgroundColor)
				.Grid(),
				
				new Button().Content("Play")
				.Width(60).Height(60).Assign(out var btn)
				.HorizontalAlignment(HorizontalAlignment.Center)
				.VerticalAlignment(VerticalAlignment.Center)
				.Background("#DE5154")
				.CornerRadius(30),
				MusicInfo(vm).Grid(1,0),
				PlayerCommands().Grid(2,0)
			).RowDefinitions("*, 48");
			btn.Click += (s, e) => storyboard.Begin();

		return grid;
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
