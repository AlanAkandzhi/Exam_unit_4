using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class WeatherLogger
{
    private static readonly string dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Exam_Unit_4", "weather_data.json");

    // Structure to store both user and YR weather data together.
    public class WeatherEntry
    {
        public WeatherData userWeatherData { get; set; }
        public WeatherData yrWeatherData { get; set; }
    }

    public static void SaveWeatherData(WeatherData userWeatherData, WeatherData yrWeatherData)
    {
        var weatherLog = LoadData();
        weatherLog.Add(new WeatherEntry { userWeatherData = userWeatherData, yrWeatherData = yrWeatherData });

        EnsureDirectoryExists(dataFilePath); // Ensure directory exists before writing
        File.WriteAllText(dataFilePath, JsonSerializer.Serialize(weatherLog));
    }

    public static List<WeatherEntry> LoadData()
    {
        if (!File.Exists(dataFilePath))
        {
            return new List<WeatherEntry>();
        }

        string json = File.ReadAllText(dataFilePath);
        return JsonSerializer.Deserialize<List<WeatherEntry>>(json);
    }

    public static void GenerateDailyReport()
    {
        var weatherLog = LoadData();
        var todayData = weatherLog.Where(w => w.userWeatherData.Date.Date == DateTime.Today).ToList();

        if (!todayData.Any())
        {
            Console.WriteLine("No data for today.");
            return;
        }

        foreach (var entry in todayData)
        {
            DisplayWeatherData(entry.userWeatherData, entry.yrWeatherData);
        }
    }

    public static void GenerateWeeklyReport()
    {
        var weatherLog = LoadData();
        var startDate = DateTime.Today.AddDays(-7);

        var weeklyData = weatherLog.Where(w => w.userWeatherData.Date.Date >= startDate).ToList();

        if (!weeklyData.Any())
        {
            Console.WriteLine("No data for this week.");
            return;
        }

        foreach (var entry in weeklyData)
        {
            DisplayWeatherData(entry.userWeatherData, entry.yrWeatherData);
        }
    }

    public static void GenerateMonthlyReport()
    {
        var weatherLog = LoadData();
        var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        var monthlyData = weatherLog.Where(w => w.userWeatherData.Date.Date >= startDate).ToList();

        if (!monthlyData.Any())
        {
            Console.WriteLine("No data for this month.");
            return;
        }

        foreach (var entry in monthlyData)
        {
            DisplayWeatherData(entry.userWeatherData, entry.yrWeatherData);
        }
    }

   private static void DisplayWeatherData(WeatherData userWeatherData, WeatherData yrWeatherData)
{
    Console.WriteLine($"Date: {userWeatherData?.Date.ToShortDateString() ?? "Unknown"}");

    if (userWeatherData != null)
    {
        Console.WriteLine($"User Temp: {userWeatherData.Temperature} - YR Temp: {yrWeatherData?.Temperature ?? 0}");
        Console.WriteLine($"User Humidity: {userWeatherData.Humidity} - YR Humidity: {yrWeatherData?.Humidity ?? 0}");
        Console.WriteLine($"User Wind Speed: {userWeatherData.WindSpeed} - YR Wind Speed: {yrWeatherData?.WindSpeed ?? 0}");
        
        Console.WriteLine("Differences:");
        Console.WriteLine($"Temperature Difference: {userWeatherData.Temperature - (yrWeatherData?.Temperature ?? 0)}");
        Console.WriteLine($"Humidity Difference: {userWeatherData.Humidity - (yrWeatherData?.Humidity ?? 0)}");
        Console.WriteLine($"Wind Speed Difference: {userWeatherData.WindSpeed - (yrWeatherData?.WindSpeed ?? 0)}");
    }
    else
    {
        Console.WriteLine("No user data available for display.");
    }
    Console.WriteLine();
}

    private static void EnsureDirectoryExists(string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
