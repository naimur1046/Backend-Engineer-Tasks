namespace Controllers.Services;

public interface IDateService
{
   int CalculateDaysBetween(string startDate, string endDate);
   (int year, int month, int day) ParseDate(string date);
   bool IsValidDate(string date);
   bool IsStartDateBeforeOrEqualEndDate(string startDate, string endDate);
}

public class DateService : IDateService
{
   private static readonly int[] DaysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
   
   public int CalculateDaysBetween(string startDate, string endDate)
   {
       var (startYear, startMonth, startDay) = ParseDate(startDate);
       var (endYear, endMonth, endDay) = ParseDate(endDate);
       var startDays = CountDaysFromEpoch(startYear, startMonth, startDay);
       var endDays = CountDaysFromEpoch(endYear, endMonth, endDay);
       
       return Math.Abs(endDays - startDays);
   }

   public (int year, int month, int day) ParseDate(string date)
   {
       var parts = date.Split('-');
       if (parts.Length != 3)
       {
           throw new ArgumentException("Invalid date format. Expected YYYY-MM-DD", nameof(date));
       }

       if (!int.TryParse(parts[0], out var year) ||
           !int.TryParse(parts[1], out var month) ||
           !int.TryParse(parts[2], out var day))
       {
           throw new ArgumentException("Invalid date format. Expected YYYY-MM-DD", nameof(date));
       }
       
       if (!IsValidDateComponents(year, month, day))
       {
           throw new ArgumentException("Invalid date values", nameof(date));
       }

       return (year, month, day);
   }

   public bool IsValidDate(string date)
   {
       try
       {
           var parts = date.Split('-');
           if (parts.Length != 3) return false;

           if (!int.TryParse(parts[0], out var year) ||
               !int.TryParse(parts[1], out var month) ||
               !int.TryParse(parts[2], out var day))
           {
               return false;
           }

           return IsValidDateComponents(year, month, day);
       }
       catch
       {
           return false;
       }
   }

   public bool IsStartDateBeforeOrEqualEndDate(string startDate, string endDate)
   {
       var (startYear, startMonth, startDay) = ParseDate(startDate);
       var (endYear, endMonth, endDay) = ParseDate(endDate);

       var startDays = CountDaysFromEpoch(startYear, startMonth, startDay);
       var endDays = CountDaysFromEpoch(endYear, endMonth, endDay);

       return startDays <= endDays;
   }

   private bool IsValidDateComponents(int year, int month, int day)
   {
       if (year < 1 || month < 1 || month > 12 || day < 1)
           return false;
       
       var maxDay = GetDaysInMonth(year, month);
       
       return day <= maxDay;
   }
   
   private int CountDaysFromEpoch(int year, int month, int day)
   {
       int totalDays = 0;
       
       for (int y = 1; y < year; y++)
       {
           totalDays += IsLeapYear(y) ? 366 : 365;
       }
       
       for (int m = 1; m < month; m++)
       {
           totalDays += GetDaysInMonth(year, m);
       }
       
       totalDays += day;
       
       return totalDays;
   }


   private bool IsLeapYear(int year)
   {
       if (year % 400 == 0) return true;
       if (year % 100 == 0) return false;
       if (year % 4 == 0) return true;
       return false;
   }

   private int GetDaysInMonth(int year, int month)
   {
       if (month == 2 && IsLeapYear(year))
       {
           return 29;
       }
       
       return DaysInMonth[month - 1];
   }
}

