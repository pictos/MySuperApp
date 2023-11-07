using Uno.Extensions.Reactive;

namespace MySuperApp.Pages;

public partial class ComicsPage : Page
{
    const string comicUrl = "https://xkcd.com/927/";

    public ComicsPage()
    {
        this.DataContext(new BindableWeatherModel(new WeatherService()), (page, vm) =>
        {
            new FeedView()
                .Background(Colors.Black)
                .Source(() => vm.CurrentWeather)
                .DataTemplate(s =>
                {
                    new StackPanel().DataContext(() => vm.Model)
                        .Children
                        (
                            new TextBlock()
                                .Text(x => x.Bind("Temperature")
                                .Mode(BindingMode.TwoWay))
                                .VerticalAlignment(VerticalAlignment.Center)
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .FontSize(32),
                            new Button().Content("Refresh")
                        );
                })
                .ProgressTemplate<StackPanel>(s => s.Children
                (
                    new TextBlock().Text("Loading...")
                ));

        });
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


