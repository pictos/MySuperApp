using Uno.Extensions.Reactive;

namespace MySuperApp.Pages;

public partial class ComicsPage : Page
{
	const string comicUrl = "https://imgs.xkcd.com/comics/standards_2x.png";

	public ComicsPage()
	{
		this.DataContext(new BindableWeatherModel(new WeatherService()), (page, vm) =>
		{
			page.Padding(58)
				.Background(Colors.Fuchsia)
				.Content
				(
					new Image()
						.Source(new Uri(comicUrl))
						.HorizontalAlignment(HorizontalAlignment.Center)
						.VerticalAlignment(VerticalAlignment.Center)
						// .Height(300)
						// .Width(300)
						.Stretch(Stretch.Uniform)
						//.Background(() => Colors.Black)
						// .HandleLoadError((s, e) =>
						// {
						//     _ = e.ErrorMessage;
						// })
					
					// new FeedView()
					//     .Background(Colors.Black)
					//     .Source(() => vm.CurrentWeather)
					//     .DataTemplate(s => FeedTemplate())
				);
		});
	}
	


	static StackPanel FeedTemplate()
	{
		return new StackPanel()
			.Children
			 (
				 new TextBlock()
					 //This is generating a build error
					 //.DataContext( x => x.Bind("Data"))
					 .Text(x => x.Bind("Temperature")
						 .Mode(BindingMode.TwoWay))
					 .VerticalAlignment(VerticalAlignment.Center)
					 .HorizontalAlignment(HorizontalAlignment.Center)
					 .FontSize(32),
				 new Button().Content("Refresh")
			 );   
	}
}


public partial record WeatherModel(IWeatherService WeatherService)
{
	public IFeed<WeatherInfo> CurrentWeather => Feed.Async(WeatherService.GetCurrentWeather);
}

public record WeatherInfo(int Temperature);

public interface IWeatherService
{
	ValueTask<WeatherInfo> GetCurrentWeather(CancellationToken ct);
}

public class WeatherService : IWeatherService
{
	public async ValueTask<WeatherInfo> GetCurrentWeather(CancellationToken ct)
	{
		await Task.Delay(TimeSpan.FromSeconds(1), ct);
		return new WeatherInfo(new Random().Next(-40, 40));
	}
}


