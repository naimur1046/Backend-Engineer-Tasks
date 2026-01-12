using Controllers.Services;
using Xunit;

namespace Task.Test.Services;

public class DateServiceTests
{
   private readonly DateService _service;

   public DateServiceTests()
   {
       _service = new DateService();
   }

   #region CalculateDaysBetween - Basic Cases

   [Fact]
   public void CalculateDaysBetween_SameDate_ReturnsZero()
   {
       var result = _service.CalculateDaysBetween("2024-01-15", "2024-01-15");
       Assert.Equal(0, result);
   }

   [Fact]
   public void CalculateDaysBetween_ConsecutiveDays_ReturnsOne()
   {
       var result = _service.CalculateDaysBetween("2024-01-01", "2024-01-02");
       Assert.Equal(1, result);
   }

   [Fact]
   public void CalculateDaysBetween_OneWeek_ReturnsSeven()
   {
       var result = _service.CalculateDaysBetween("2024-01-01", "2024-01-08");
       Assert.Equal(7, result);
   }

   [Fact]
   public void CalculateDaysBetween_OneMonth_ReturnsCorrectDays()
   {
       var result = _service.CalculateDaysBetween("2024-01-01", "2024-02-01");
       Assert.Equal(31, result);
   }

   #endregion
   
   #region CalculateDaysBetween - Order Independence
   
   [Fact]
   public void CalculateDaysBetween_ReversedDates_ReturnsSameAbsoluteValue()
   {
       var result1 = _service.CalculateDaysBetween("2024-01-01", "2024-01-15");
       var result2 = _service.CalculateDaysBetween("2024-01-15", "2024-01-01");

       Assert.Equal(14, result1);
       Assert.Equal(result1, result2);
   }

   #endregion

   #region CalculateDaysBetween - Leap Year Handling

   [Fact]
   public void CalculateDaysBetween_LeapYear_FebruaryHas29Days()
   {
       var result = _service.CalculateDaysBetween("2024-02-01", "2024-03-01");
       Assert.Equal(29, result);
   }
   
   [Fact]
   public void CalculateDaysBetween_NonLeapYear_FebruaryHas28Days()
   {
       var result = _service.CalculateDaysBetween("2023-02-01", "2023-03-01");
       Assert.Equal(28, result);
   }

   [Fact]
   public void CalculateDaysBetween_LeapYear2024_FullYearIs366Days()
   {
       var result = _service.CalculateDaysBetween("2024-01-01", "2025-01-01");
       Assert.Equal(366, result);
   }
   
   [Fact]
   public void CalculateDaysBetween_NonLeapYear2023_FullYearIs365Days()
   {
       var result = _service.CalculateDaysBetween("2023-01-01", "2024-01-01");
       Assert.Equal(365, result);
   }


   [Fact]
   public void CalculateDaysBetween_Century_Year2000IsLeapYear()
   {
       var result = _service.CalculateDaysBetween("2000-02-01", "2000-03-01");
       Assert.Equal(29, result);
   }

   [Fact]
   public void CalculateDaysBetween_Century_Year1900IsNotLeapYear()
   {
       var result = _service.CalculateDaysBetween("1900-02-01", "1900-03-01");
       Assert.Equal(28, result);
   }

   #endregion
   
   #region CalculateDaysBetween - Across Years
   
   [Fact]
   public void CalculateDaysBetween_AcrossYears_ReturnsCorrectDays()
   {
       var result = _service.CalculateDaysBetween("2023-12-31", "2024-01-01");
       Assert.Equal(1, result);
   }

   [Fact]
   public void CalculateDaysBetween_MultipleYears_ReturnsCorrectDays()
   {
       var result = _service.CalculateDaysBetween("2020-01-01", "2023-01-01");
       Assert.Equal(1096, result);
   }
   
   #endregion
   
   #region ParseDate - Valid Dates

   [Theory]
   [InlineData("2024-01-15", 2024, 1, 15)]
   [InlineData("2000-12-31", 2000, 12, 31)]
   [InlineData("1999-06-05", 1999, 6, 5)]
   public void ParseDate_ValidDate_ReturnsCorrectComponents(string date, int expectedYear, int expectedMonth, int expectedDay)
   {
       var (year, month, day) = _service.ParseDate(date);
       
       Assert.Equal(expectedYear, year);
       Assert.Equal(expectedMonth, month);
       Assert.Equal(expectedDay, day);
   }
   
   #endregion

   #region ParseDate - Invalid Format

   [Theory]
   [InlineData("2024/01/15")]
   [InlineData("15-01-2024")]
   [InlineData("01-15-2024")]
   [InlineData("20240115")]
   [InlineData("invalid")]
   [InlineData("")]
   public void ParseDate_InvalidFormat_ThrowsArgumentException(string date)
   {
       Assert.Throws<ArgumentException>(() => _service.ParseDate(date));
   }

   #endregion

   #region ParseDate - Invalid Date Values
   
   [Theory]
   [InlineData("2024-00-15")]
   [InlineData("2024-13-15")]
   [InlineData("2024-01-00")]
   [InlineData("2024-01-32")]
   [InlineData("2024-02-30")]
   [InlineData("2023-02-29")]
   [InlineData("0000-01-15")]
   public void ParseDate_InvalidDateValues_ThrowsArgumentException(string date)
   {
       Assert.Throws<ArgumentException>(() => _service.ParseDate(date));
   }
   
   #endregion

   #region IsValidDate - Valid Dates
   
   [Theory]
   [InlineData("2024-01-01")]
   [InlineData("2024-12-31")]
   [InlineData("2024-02-29")]
   [InlineData("2023-02-28")]
   [InlineData("2000-02-29")]
   public void IsValidDate_ValidDate_ReturnsTrue(string date)
   {
       var result = _service.IsValidDate(date);
       Assert.True(result);
   }
   
   #endregion

   #region IsValidDate - Invalid Dates

   [Theory]
   [InlineData("2024/01/01")]
   [InlineData("invalid")]
   [InlineData("")]
   [InlineData("2024-00-01")]
   [InlineData("2024-13-01")]
   [InlineData("2024-01-32")]
   [InlineData("2023-02-29")]
   [InlineData("1900-02-29")]
   public void IsValidDate_InvalidDate_ReturnsFalse(string date)
   {
       var result = _service.IsValidDate(date);
       Assert.False(result);
   }
   
   #endregion

   #region Month Days Validation

   [Theory]
   [InlineData("2024-01-31")]
   [InlineData("2024-03-31")]
   [InlineData("2024-05-31")]
   [InlineData("2024-07-31")]
   [InlineData("2024-08-31")]
   [InlineData("2024-10-31")]
   [InlineData("2024-12-31")]
   public void IsValidDate_31DayMonths_LastDayIsValid(string date)
   {
       Assert.True(_service.IsValidDate(date));
   }
   
   [Theory]
   [InlineData("2024-04-30")]
   [InlineData("2024-06-30")]
   [InlineData("2024-09-30")]
   [InlineData("2024-11-30")]
   public void IsValidDate_30DayMonths_LastDayIsValid(string date)
   {
       Assert.True(_service.IsValidDate(date));
   }

   [Theory]
   [InlineData("2024-04-31")]
   [InlineData("2024-06-31")]
   [InlineData("2024-09-31")]
   [InlineData("2024-11-31")]
   public void IsValidDate_30DayMonths_Day31IsInvalid(string date)
   {
       Assert.False(_service.IsValidDate(date));
   }
   
   #endregion

   #region Edge Cases

   [Fact]
   public void CalculateDaysBetween_DistantDates_ReturnsCorrectDays()
   {
       var result = _service.CalculateDaysBetween("1900-01-01", "2000-01-01");
       Assert.Equal(36524, result);
   }

   [Fact]
   public void CalculateDaysBetween_WithinSameMonth_ReturnsCorrectDays()
   {
       var result = _service.CalculateDaysBetween("2024-06-10", "2024-06-20");
       Assert.Equal(10, result);
   }

   #endregion

   #region IsStartDateBeforeOrEqualEndDate

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_StartBeforeEnd_ReturnsTrue()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-01-01", "2024-01-15");
       Assert.True(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_SameDate_ReturnsTrue()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-01-15", "2024-01-15");
       Assert.True(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_StartAfterEnd_ReturnsFalse()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-01-15", "2024-01-01");
       Assert.False(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_AcrossYears_ReturnsTrue()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2023-12-31", "2024-01-01");
       Assert.True(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_AcrossYearsReversed_ReturnsFalse()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-01-01", "2023-12-31");
       Assert.False(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_DifferentMonthsSameYear_ReturnsTrue()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-03-15", "2024-06-20");
       Assert.True(result);
   }

   [Fact]
   public void IsStartDateBeforeOrEqualEndDate_DifferentMonthsSameYearReversed_ReturnsFalse()
   {
       var result = _service.IsStartDateBeforeOrEqualEndDate("2024-06-20", "2024-03-15");
       Assert.False(result);
   }

   #endregion
}
