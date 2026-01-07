using System.Text.Json;
using Controllers.Models.Response;

namespace Controllers.Services;

public interface ITemperatureService
{
   Task<TemperatureResponse> GetTemperatureStatsAsync(string startDate, string endDate);
}


public class TemperatureService : ITemperatureService
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly INumberService _numberService;
   private readonly IDateService _dateService;
   
   private const double Latitude = 23.8103;
   private const double Longitude = 90.4125;
   
   public TemperatureService(
       IHttpClientFactory httpClientFactory,
       INumberService numberService,
       IDateService dateService)
   {
       _httpClientFactory = httpClientFactory;
       _numberService = numberService;
       _dateService = dateService;
   }


   public async Task<TemperatureResponse> GetTemperatureStatsAsync(string startDate, string endDate)
   {
       _dateService.ParseDate(startDate);
       _dateService.ParseDate(endDate);
       
       var temperatures = await FetchTemperaturesFromApiAsync(startDate, endDate);
       
       if (temperatures.Count == 0)
       {
           throw new InvalidOperationException("No temperature data available for the specified date range");
       }

       var min = temperatures.Min();
       var max = temperatures.Max();
       var average = Math.Round(temperatures.Average(), 2);

       return new TemperatureResponse
       {
           Min = min,
           Max = max,
           Average = average,
           MinText = ConvertTemperatureToText(min),
           MaxText = ConvertTemperatureToText(max),
           AverageText = ConvertTemperatureToText(average)
       };
   }
   
   private async Task<List<double>> FetchTemperaturesFromApiAsync(string startDate, string endDate)
   {
       var client = _httpClientFactory.CreateClient("default");

       var url = $"https://api.open-meteo.com/v1/forecast?" +
                 $"latitude={Latitude}&longitude={Longitude}" +
                 $"&daily=temperature_2m_mean" +
                 $"&start_date={startDate}&end_date={endDate}" +
                 $"&timezone=Asia/Dhaka";
       
       var response = await client.GetAsync(url);
       response.EnsureSuccessStatusCode();

       var json = await response.Content.ReadAsStringAsync();
       var data = JsonSerializer.Deserialize<OpenMeteoResponse>(json, new JsonSerializerOptions
       {
           PropertyNameCaseInsensitive = true
       });


       if (data?.Daily?.Temperature_2m_mean == null)
       {
           return new List<double>();
       }
       
       return data.Daily.Temperature_2m_mean
           .Where(t => t.HasValue)
           .Select(t => t!.Value)
           .ToList();
   }

   private string ConvertTemperatureToText(double temperature)
   {
       var isNegative = temperature < 0;
       var absValue = Math.Abs(temperature);

       
       var roundedValue = Math.Round(absValue, 2);

       
       var words = _numberService.ConvertToWords((decimal)roundedValue);

       
       var prefix = isNegative ? "minus" : "positive";

       return $"{prefix} {words}";
   }
}


public class OpenMeteoResponse
{
   public DailyData? Daily { get; set; }
}

public class DailyData
{
   public List<double?>? Temperature_2m_mean { get; set; }
}

