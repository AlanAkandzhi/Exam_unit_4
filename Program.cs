using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to the Weather Log Application");

        while (true)
        {
            Console.WriteLine("1. Log today's weather");
            Console.WriteLine("2. View reports");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await LogTodayWeather();
                    break;
                case "2":
                    ViewReports();
                    break;
                case "3":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task LogTodayWeather()
    {
        Console.Write("Enter today's temperature: ");
        double temperature = double.Parse(Console.ReadLine());

        Console.Write("Enter today's humidity: ");
        double humidity = double.Parse(Console.ReadLine());

        Console.Write("Enter today's wind speed: ");
        double windSpeed = double.Parse(Console.ReadLine());

        WeatherData userWeatherData = new WeatherData
        {
            Date = DateTime.Today,
            Temperature = temperature,
            Humidity = humidity,
            WindSpeed = windSpeed,
        };

        WeatherData yrWeatherData = await YRService.GetYRWeather();

        WeatherLogger.SaveWeatherData(userWeatherData, yrWeatherData);

        Console.WriteLine("Weather data logged successfully.");
        DisplayDifference(userWeatherData, yrWeatherData);
    }

    static void DisplayDifference(WeatherData userData, WeatherData yrData)
    {
        Console.WriteLine("Differences:");
        Console.WriteLine($"Temperature: {userData.Temperature - yrData.Temperature}");
        Console.WriteLine($"Humidity: {userData.Humidity - yrData.Humidity}");
        Console.WriteLine($"Wind Speed: {userData.WindSpeed - yrData.WindSpeed}");
    }

    static void ViewReports()
    {
        Console.WriteLine("Choose a report:");
        Console.WriteLine("1. Daily");
        Console.WriteLine("2. Weekly");
        Console.WriteLine("3. Monthly");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                WeatherLogger.GenerateDailyReport();
                break;
            case "2":
                WeatherLogger.GenerateWeeklyReport();
                break;
            case "3":
                WeatherLogger.GenerateMonthlyReport();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
