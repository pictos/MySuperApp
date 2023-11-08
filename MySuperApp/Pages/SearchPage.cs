using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySuperApp.Services;

namespace MySuperApp.Pages;


readonly record struct SearchResult(string Title, string AlbumUrl, string Duration);
public partial class SearchPage : Page
{
	public SearchPage()
	{
		this.DataContext(new SearchViewModel(), (page, vm) =>
		{
			page
			.Background(Colors.White)
			.Content(MainContent(vm))
			.Padding(0, 58, 0, 20);
		});
	}

	void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
	{
		if (e.Key != Windows.System.VirtualKey.Enter)
			return;
		e.Handled = true;
		var textBlock = (TextBox)sender;
		RemoveFocus(textBlock);
		((SearchViewModel)DataContext).SearchExecuteCommand.ExecuteAsync(null);
	}

	void RemoveFocus(object sender)
	{
		var control = (Control)sender;
		var isTabStop = control.IsTabStop;
		control.IsTabStop = false;
		control.IsEnabled = false;
		control.IsEnabled = true;
		control.IsTabStop = isTabStop;
	}

	enum PageRows
	{
		Search = 0,
		ListView = 1,
		Button = 2
	}

	Grid MainContent(SearchViewModel vm)
	{
		var grid = new Grid().RowDefinitions("auto, *, auto")
			.Children
			(
				new ProgressRing()
					.Width(100)
					.Height(100)
					.HorizontalAlignment(HorizontalAlignment.Center)
					.VerticalAlignment(VerticalAlignment.Center)
					.IsActive(() => vm.SearchExecuteCommand.IsRunning)
					.Grid(rowSpan: 3),
				new TextBox()
					.PlaceholderText("Search...")
					.Grid(PageRows.Search, 0)
					.Foreground(Colors.Black)
					.Margin(5)
					.Text(x => x.Bind(() => vm.Query).Mode(BindingMode.TwoWay))
					.Assign(out var txtBlock),
				new ListView()
					.ItemsSource(() => vm.Results)
					.Grid(PageRows.ListView, 0)
					.Visibility(builder => SetVisibility(builder, vm))
					.ItemTemplate<SearchResult>
					(result => 
						new Grid().Children
						(
							new MediaCell()
						)
						//new Grid().Children
						//	(
						//		new Image().Source(() => result.ImageUrl).Grid(row: 0, rowSpan: 2)
						//			.Stretch(Stretch.Uniform),
						//		new StackPanel()
						//			.Padding(5)
						//			.Background("#99000000")
						//			.Children
						//			(
						//				new TextBlock()
						//					.Text(() => result.Title)
						//					.Foreground(Colors.White)
						//					.VerticalAlignment(VerticalAlignment.Center)
						//					.TextWrapping(TextWrapping.WrapWholeWords),
						//				new TextBlock().Foreground(Colors.White).Text(() => result.Duration)
						//			).Grid(1, 0)
						//	).RowDefinitions("* , auto")
						//	.RowSpacing(16)
					),
				new Button()
					.Content("Search")
					.Grid(PageRows.Button, 0)
					.MinWidth(100)
					.HorizontalAlignment(HorizontalAlignment.Center)
					.Assign(out var btn)
					.Command(() => vm.SearchExecuteCommand)
			);

		txtBlock.KeyUp += TextBox_KeyUp;
		SetupVisualStateManager();
		return grid;

		Grid SetupVisualStateManager()
		{
			btn.Click += (_, __) =>
			{
				VisualStateManager.GoToState(this, "Entered", true);
			};

			btn.PointerExited += (_, __) =>
			{
				VisualStateManager.GoToState(this, "Exited", true);
			};

			btn.PointerEntered += (_, __) =>
			{
				VisualStateManager.GoToState(this, "Entered", true);
			};

			grid.VisualStateManager
			(b =>
				b.Group("SearchButtonState", gb =>
					gb.State("Entered", sb =>
							sb.Setters(btn, e => e.Width(App.Bounds.Width * 0.6)))
						.State("Exited", sb =>
							sb.Setters(btn, e => e.Width(100))))
			);

			return grid;
		}
	}

	static void SetVisibility(IDependencyPropertyBuilder<Visibility> builder, SearchViewModel vm)
	{
		builder
		.Bind(() => vm.SearchExecuteCommand.IsRunning)
		.Convert(x => x ? Visibility.Collapsed : Visibility.Visible);
	}
}

partial class SearchViewModel : ObservableObject
{
	[ObservableProperty]
	string query = string.Empty;

	public ObservableCollection<SearchResult> Results { get; } = new();

	static AllServices MediaService => AllServices.Current;

	[RelayCommand]
	async Task SearchExecute()
	{
		if (string.IsNullOrWhiteSpace(Query))
		{
			return;
		}

		Results.Clear();

		await foreach (var item in MediaService.SearchMediaAsync(Query))
		{
			Results.Add(item);
		}
	}
}


//partial class SearchModel
//{
//	public IListFeed<SearchResult> Results => ListFeed.Async(GetResultsAsync);

//	static ValueTask<ImmutableList<SearchResult>> GetResultsAsync(CancellationToken t = default) => new(GetResults().ToImmutableList());

//	static IEnumerable<SearchResult> GetResults()
//	{
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
//	}
//}
