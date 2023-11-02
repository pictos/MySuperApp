using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySuperApp.Services;

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


		//this.DataContext(new BindableSearchModel(), (page, vm) =>
		//{
		//    page
		//    .Background(Colors.White)
		//    .Content(MVUXVersion(vm))
		//    .Padding(58);
		//});
	}


	FeedView MVUXVersion(BindableSearchModel model)
	{
		return new FeedView().Source(() => model.Results)
			.DataTemplate(d =>
			{
				new StackPanel().Children(new TextBlock().Text("Title")).Background(Colors.Fuchsia);
			})
			.Background(Colors.Black);
	}

	static Grid MainContent(SearchViewModel vm) => new Grid().RowDefinitions("auto, *, auto")
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
				.Grid(0, 0)
				.Foreground(Colors.Black)
				.Margin(5)
				.Text(x => x.Bind(() => vm.Query).Mode(BindingMode.TwoWay)),
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
						.RowSpacing(16)
				)

				//TODO: report bug where Visibility breaks if appears before the Itemtemplate method
				.Visibility(builder => SetVisibility(builder, vm)),
			new Button()
				.Content("Search")
				.Grid(2, 0)
				.MinWidth(100)
				.HorizontalAlignment(HorizontalAlignment.Center)
				.Assign(out var btn)
				.Command(() => vm.SearchExecuteCommand)
			   );

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


partial class SearchModel
{
	public IListFeed<SearchResult> Results => ListFeed.Async(GetResultsAsync);

	static ValueTask<ImmutableList<SearchResult>> GetResultsAsync(CancellationToken t = default) => new(GetResults().ToImmutableList());

	static IEnumerable<SearchResult> GetResults()
	{
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
		yield return new SearchResult("Title 1", "https://avatars.githubusercontent.com/u/20712372?v=4", "03:30");
	}
}
