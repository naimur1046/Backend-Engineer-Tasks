using System.Net;
using System.Text.Json;
using Controllers.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace Task.Test.Services;

public class TemperatureServiceTests
{
   private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
   private readonly Mock<INumberService> _numberServiceMock;
   private readonly Mock<IDateService> _dateServiceMock;
   private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
   
   public TemperatureServiceTests()
   {
       _httpClientFactoryMock = new Mock<IHttpClientFactory>();
       _numberServiceMock = new Mock<INumberService>();
       _dateServiceMock = new Mock<IDateService>();
       _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
   }
   
   private TemperatureService CreateService()
   {
       return new TemperatureService(
           _httpClientFactoryMock.Object,
           _numberServiceMock.Object,
           _dateServiceMock.Object);
   }
   
   private void SetupHttpClient(string responseJson, HttpStatusCode statusCode = HttpStatusCode.OK)
   {
       _httpMessageHandlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = statusCode,
               Content = new StringContent(responseJson)
           });
       
       var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
       _httpClientFactoryMock
           .Setup(f => f.CreateClient("default"))
           .Returns(httpClient);
   }
   
   #region GetTemperatureStatsAsync - Basic Functionality
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_ValidData_ReturnsCorrectStats()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 20.0, 25.0, 30.0 }
           }
       });

       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(It.IsAny<decimal>())).Returns("test");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-03");
       
       Assert.Equal(20.0, result.Min);
       Assert.Equal(30.0, result.Max);
       Assert.Equal(25.0, result.Average);
   }

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_SingleTemperature_ReturnsAllSameValues()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 25.0 }
           }
       });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(It.IsAny<decimal>())).Returns("test");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01");
       
       Assert.Equal(25.0, result.Min);
       Assert.Equal(25.0, result.Max);
       Assert.Equal(25.0, result.Average);
   }

   #endregion

   #region GetTemperatureStatsAsync - Text Conversion
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_PositiveTemperature_ReturnsPositivePrefix()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 25.5 }
           }
       });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(25.5m)).Returns("twenty five point five zero");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01");

       Assert.StartsWith("positive", result.MinText);
       Assert.StartsWith("positive", result.MaxText);
       Assert.StartsWith("positive", result.AverageText);
   }

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_NegativeTemperature_ReturnsMinusPrefix()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { -5.4 }
           }
       });

       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(5.4m)).Returns("five point four zero");
       
       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01");
       
       Assert.StartsWith("minus", result.MinText);
       Assert.StartsWith("minus", result.MaxText);
       Assert.StartsWith("minus", result.AverageText);
   }

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_MixedTemperatures_ReturnsCorrectPrefixes()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { -5.4, 1.3 }
           }
       });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(5.4m)).Returns("five point four zero");
       _numberServiceMock.Setup(n => n.ConvertToWords(1.3m)).Returns("one point three zero");
       _numberServiceMock.Setup(n => n.ConvertToWords(2.05m)).Returns("two point zero five");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-02");

       Assert.Equal(-5.4, result.Min);
       Assert.Equal(1.3, result.Max);
       Assert.StartsWith("minus", result.MinText);
       Assert.StartsWith("positive", result.MaxText);
   }
   
   #endregion
   
   #region GetTemperatureStatsAsync - Date Validation
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_InvalidStartDate_ThrowsArgumentException()
   {
       _dateServiceMock
           .Setup(d => d.ParseDate("invalid"))
           .Throws(new ArgumentException("Invalid date"));
       
       var service = CreateService();
       
       await Assert.ThrowsAsync<ArgumentException>(() =>
           service.GetTemperatureStatsAsync("invalid", "2024-01-01"));
   }
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_InvalidEndDate_ThrowsArgumentException()
   {
       _dateServiceMock
           .Setup(d => d.ParseDate("2024-01-01"))
           .Returns((2024, 1, 1));
       _dateServiceMock
           .Setup(d => d.ParseDate("invalid"))
           .Throws(new ArgumentException("Invalid date"));

       var service = CreateService();

       await Assert.ThrowsAsync<ArgumentException>(() =>
           service.GetTemperatureStatsAsync("2024-01-01", "invalid"));
   }

   #endregion
   
   #region GetTemperatureStatsAsync - No Data Handling
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_NoTemperatureData_ThrowsInvalidOperationException()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { }
           }
       });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       
       var service = CreateService();

       await Assert.ThrowsAsync<InvalidOperationException>(() =>
           service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01"));
   }
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_NullDailyData_ThrowsInvalidOperationException()
   {
       var responseJson = JsonSerializer.Serialize(new { daily = (object?)null });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       
       var service = CreateService();
       
       await Assert.ThrowsAsync<InvalidOperationException>(() =>
           service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01"));
   }

   #endregion

   #region GetTemperatureStatsAsync - Average Calculation

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_AverageIsRoundedToTwoDecimals()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 10.0, 20.0, 30.0, 15.0 }
           }
       });

       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(It.IsAny<decimal>())).Returns("test");
       
       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-04");
       
       Assert.Equal(18.75, result.Average);
   }

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_AverageWithNullValues_IgnoresNulls()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 20.0, null, 30.0 }
           }
       });
       
       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(It.IsAny<decimal>())).Returns("test");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-03");


       Assert.Equal(20.0, result.Min);
       Assert.Equal(30.0, result.Max);
       Assert.Equal(25.0, result.Average);
   }
   
   #endregion
   
   #region GetTemperatureStatsAsync - API Error Handling

   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_ApiReturnsError_ThrowsHttpRequestException()
   {
       SetupHttpClient("{}", HttpStatusCode.InternalServerError);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       
       var service = CreateService();

       await Assert.ThrowsAsync<HttpRequestException>(() =>
           service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01"));
   }
   
   #endregion
   
   #region Temperature Text Format
   
   [Fact]
   public async System.Threading.Tasks.Task GetTemperatureStatsAsync_TextFormat_ContainsCorrectWords()
   {
       var responseJson = JsonSerializer.Serialize(new
       {
           daily = new
           {
               temperature_2m_mean = new double?[] { 1.3 }
           }
       });

       SetupHttpClient(responseJson);
       _dateServiceMock.Setup(d => d.ParseDate(It.IsAny<string>())).Returns((2024, 1, 1));
       _numberServiceMock.Setup(n => n.ConvertToWords(1.3m)).Returns("one point three zero");

       var service = CreateService();
       var result = await service.GetTemperatureStatsAsync("2024-01-01", "2024-01-01");
       
       Assert.Equal("positive one point three zero", result.MaxText);
   }
   
   #endregion
}
