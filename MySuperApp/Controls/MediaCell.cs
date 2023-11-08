namespace MySuperApp.Controls;

internal partial class MediaCell : UserControl
{
	public MediaCell()
	{
		this.DataContext<SearchResult>((cell, model) =>
		{
			cell.Content(
				new Grid()
				.MinHeight(300)
				.Children
					(
						new Image().Source(() => model.AlbumUrl).Grid(row: 0, rowSpan: 2)
							.Stretch(Stretch.Uniform),
						new StackPanel()
							.Padding(5)
							.Background("#99000000")
							.Children
							(
								new TextBlock()
									.Text(() => model.Title)
									.Foreground(Colors.White)
									.VerticalAlignment(VerticalAlignment.Center)
									.TextWrapping(TextWrapping.WrapWholeWords),
								new TextBlock().Foreground(Colors.White).Text(() => model.Duration)
							).Grid(1, 0)
					).RowDefinitions("* , auto")
					.RowSpacing(16)
		);
		});
		
	}
}
