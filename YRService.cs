using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

class YRService
{
    private static readonly string YR_API_URL = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=60&lon=11";
    private static readonly int RETRY_ATTEMPTS = 3;

    public static async Task<WeatherData> GetYRWeather()
    {
        using (HttpClient client = new HttpClient())
        {
            for (int i = 0; i < RETRY_ATTEMPTS; i++)
            {
                try
                {
                    string response = await client.GetStringAsync(YR_API_URL);

                    var weather = JsonSerializer.Deserialize<YRWeatherResponse>(response);

                    return new WeatherData
                    {
                        Date = DateTime.Now,
                        Temperature = weather.GetTemperature(),
                        Humidity = weather.GetHumidity(),
                        WindSpeed = weather.GetWindSpeed(),
                    };
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error fetching data from YR API (Attempt {i + 1}/{RETRY_ATTEMPTS}): {ex.Message}");
                    
                    if (i == RETRY_ATTEMPTS - 1)
                    {
                        Console.WriteLine("Maximum retry attempts reached. Using default values or aborting.");
                        return null;
                    }
                }
            }
        }

        return null;
    }
}


class YRWeatherResponse
{
    public WeatherTimeseries[] timeseries { get; set; }

    public double GetTemperature() => timeseries?[0]?.data?.instant?.details?.air_temperature ?? 0;
    public double GetHumidity() => timeseries?[0]?.data?.instant?.details?.relative_humidity ?? 0;
    public double GetWindSpeed() => timeseries?[0]?.data?.instant?.details?.wind_speed ?? 0;
}

class WeatherTimeseries
{
    public WeatherDataDetails data { get; set; }
}

class WeatherDataDetails
{
    public WeatherDetails instant { get; set; }
}

class WeatherDetails
{
    public WeatherInstantDetails details { get; set; }
}

class WeatherInstantDetails
{
    public double air_temperature { get; set; }
    public double relative_humidity { get; set; }
    public double wind_speed { get; set; }
}
