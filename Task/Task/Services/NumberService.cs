namespace Controllers.Services;

public interface INumberService
{
    string ConvertToWords(decimal number);
}

public class NumberService : INumberService
{
    private static readonly string[] Ones =
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };

    private static readonly string[] Tens =
    {
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    };

    public string ConvertToWords(decimal number)
    {
        if (number < 0 || number >= 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 0 and 999.99");
        }

        var integerPart = (int)Math.Truncate(number);
        var decimalPart = number - integerPart;

        var result = ConvertIntegerToWords(integerPart);

        if (decimalPart > 0)
        {
            var decimalString = number.ToString("F2").Split('.')[1];
            result += " point";
            foreach (var digit in decimalString)
            {
                result += " " + Ones[digit - '0'];
            }
        }

        return result.Trim();
    }

    private string ConvertIntegerToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 20)
            return Ones[number];

        if (number < 100)
        {
            var tens = number / 10;
            var ones = number % 10;
            return ones == 0 ? Tens[tens] : $"{Tens[tens]} {Ones[ones]}";
        }

        var hundreds = number / 100;
        var remainder = number % 100;

        if (remainder == 0)
            return $"{Ones[hundreds]} hundred";

        return $"{Ones[hundreds]} hundred {ConvertIntegerToWords(remainder)}";
    }
}