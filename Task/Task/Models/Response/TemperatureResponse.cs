namespace Controllers.Models.Response;

public class TemperatureResponse
{
    public double Min  { get; set; } 
    public double Max { get; set; }
    public double Average { get; set; }
    public string MinText { get; set; } = string.Empty;
    public string MaxText { get; set; } = string.Empty;
    public string AverageText { get; set; } = string.Empty;
}