using System.Text.Json.Serialization;
namespace Controllers.Models.Response;

public class TemperatureResponse
{
    [JsonPropertyName("min")]
    public double Min  { get; set; }
    [JsonPropertyName("max")]
    public double Max { get; set; }
    [JsonPropertyName("average")]
    public double Average { get; set; }
    [JsonPropertyName("minText")]
    public string MinText { get; set; } = string.Empty;
    [JsonPropertyName("maxText")]
    public string MaxText { get; set; } = string.Empty;
    [JsonPropertyName("averageText")]
    public string AverageText { get; set; } = string.Empty;
}