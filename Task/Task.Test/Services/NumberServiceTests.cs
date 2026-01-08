using Controllers.Services;
using Xunit;

namespace Task.Test.Services;

public class NumberServiceTests
{
   private readonly NumberService _service;

   public NumberServiceTests()
   {
       _service = new NumberService();
   }

   #region Examples from Requirements
   
   [Fact]
   public void ConvertToWords_36_ReturnsThirtySix()
   {
       var result = _service.ConvertToWords(36);
       Assert.Equal("thirty six", result);
   }
   
   [Fact]
   public void ConvertToWords_105_ReturnsOneHundredFive()
   {
       var result = _service.ConvertToWords(105);
       Assert.Equal("one hundred five", result);
   }

   [Fact]
   public void ConvertToWords_36Point40_ReturnsThirtySixPointFourZero()
   {
       var result = _service.ConvertToWords(36.40m);
       Assert.Equal("thirty six point four zero", result);
   }
   
   #endregion

   #region Single Digit Numbers (0-9)

   [Theory]
   [InlineData(0, "zero")]
   [InlineData(1, "one")]
   [InlineData(2, "two")]
   [InlineData(3, "three")]
   [InlineData(4, "four")]
   [InlineData(5, "five")]
   [InlineData(6, "six")]
   [InlineData(7, "seven")]
   [InlineData(8, "eight")]
   [InlineData(9, "nine")]
   public void ConvertToWords_SingleDigit_ReturnsCorrectWord(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }


   #endregion


   #region Teen Numbers (10-19)
   
   [Theory]
   [InlineData(10, "ten")]
   [InlineData(11, "eleven")]
   [InlineData(12, "twelve")]
   [InlineData(13, "thirteen")]
   [InlineData(14, "fourteen")]
   [InlineData(15, "fifteen")]
   [InlineData(16, "sixteen")]
   [InlineData(17, "seventeen")]
   [InlineData(18, "eighteen")]
   [InlineData(19, "nineteen")]
   public void ConvertToWords_TeenNumbers_ReturnsCorrectWord(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }
   
   #endregion
   
   #region Tens (20, 30, ... 90)
   
   [Theory]
   [InlineData(20, "twenty")]
   [InlineData(30, "thirty")]
   [InlineData(40, "forty")]
   [InlineData(50, "fifty")]
   [InlineData(60, "sixty")]
   [InlineData(70, "seventy")]
   [InlineData(80, "eighty")]
   [InlineData(90, "ninety")]
   public void ConvertToWords_RoundTens_ReturnsCorrectWord(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }

   #endregion

   #region Two-Digit Numbers

   [Theory]
   [InlineData(21, "twenty one")]
   [InlineData(45, "forty five")]
   [InlineData(67, "sixty seven")]
   [InlineData(99, "ninety nine")]
   public void ConvertToWords_TwoDigitNumbers_ReturnsCorrectWords(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }
   
   #endregion

   #region Hundreds

   [Theory]
   [InlineData(100, "one hundred")]
   [InlineData(200, "two hundred")]
   [InlineData(300, "three hundred")]
   [InlineData(500, "five hundred")]
   [InlineData(900, "nine hundred")]
   public void ConvertToWords_RoundHundreds_ReturnsCorrectWords(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }

   [Theory]
   [InlineData(101, "one hundred one")]
   [InlineData(115, "one hundred fifteen")]
   [InlineData(250, "two hundred fifty")]
   [InlineData(321, "three hundred twenty one")]
   [InlineData(999, "nine hundred ninety nine")]
   public void ConvertToWords_ThreeDigitNumbers_ReturnsCorrectWords(int number, string expected)
   {
       var result = _service.ConvertToWords(number);
       Assert.Equal(expected, result);
   }

   #endregion

   #region Decimal Numbers

   [Fact]
   public void ConvertToWords_DecimalWithZeroAfterPoint_ReturnsCorrectWords()
   {
       var result = _service.ConvertToWords(5.01m);
       Assert.Equal("five point zero one", result);
   }

   [Fact]
   public void ConvertToWords_DecimalWithTwoDigits_ReturnsCorrectWords()
   {
       var result = _service.ConvertToWords(123.45m);
       Assert.Equal("one hundred twenty three point four five", result);
   }

   [Fact]
   public void ConvertToWords_DecimalZeroPointSomething_ReturnsCorrectWords()
   {
       var result = _service.ConvertToWords(0.99m);
       Assert.Equal("zero point nine nine", result);
   }

   [Fact]
   public void ConvertToWords_SmallDecimal_ReturnsCorrectWords()
   {
       var result = _service.ConvertToWords(0.01m);
       Assert.Equal("zero point zero one", result);
   }
   
   #endregion

   #region Boundary Cases

   [Fact]
   public void ConvertToWords_Zero_ReturnsZero()
   {
       var result = _service.ConvertToWords(0);
       Assert.Equal("zero", result);
   }

   [Fact]
   public void ConvertToWords_MaxValidValue_ReturnsCorrectWords()
   {
       var result = _service.ConvertToWords(999.99m);
       Assert.Equal("nine hundred ninety nine point nine nine", result);
   }
   
   #endregion

   #region Input Validation
   
   [Fact]
   public void ConvertToWords_NegativeNumber_ThrowsArgumentOutOfRangeException()
   {
       Assert.Throws<ArgumentOutOfRangeException>(() => _service.ConvertToWords(-1));
   }
   
   [Fact]
   public void ConvertToWords_NumberEqualTo1000_ThrowsArgumentOutOfRangeException()
   {
       Assert.Throws<ArgumentOutOfRangeException>(() => _service.ConvertToWords(1000));
   }

   [Fact]
   public void ConvertToWords_NumberGreaterThan1000_ThrowsArgumentOutOfRangeException()
   {
       Assert.Throws<ArgumentOutOfRangeException>(() => _service.ConvertToWords(1001));
   }
   
   #endregion

   #region Output Format Requirements
   
   [Fact]
   public void ConvertToWords_OutputIsLowercase()
   {
       var result = _service.ConvertToWords(567);
       Assert.Equal(result.ToLower(), result);
   }

   [Fact]
   public void ConvertToWords_NoDoubleSpaces()
   {
       var result = _service.ConvertToWords(123.45m);
       Assert.DoesNotContain("  ", result);
   }

   [Fact]
   public void ConvertToWords_NoHyphens()
   {
       var result = _service.ConvertToWords(21);
       Assert.DoesNotContain("-", result);
   }
   
   [Fact]
   public void ConvertToWords_NoAndWord()
   {
       var result = _service.ConvertToWords(105);
       Assert.DoesNotContain("and", result);
   }

   #endregion
}
