using Fitradar.SharedKernel.Extensions;

namespace Fitradar.SharedKernel.Tests.Extensions;

public class DateTimeExtensionsTests
{
    // -------------------------------------------------------------------------
    // ToTimeDay
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(DayOfWeek.Sunday,    DateTimeExtensions.SUNDAY)]
    [InlineData(DayOfWeek.Monday,    DateTimeExtensions.MONDAY)]
    [InlineData(DayOfWeek.Tuesday,   DateTimeExtensions.TUESDAY)]
    [InlineData(DayOfWeek.Wednesday, DateTimeExtensions.WEDNESDAY)]
    [InlineData(DayOfWeek.Thursday,  DateTimeExtensions.THURSDAY)]
    [InlineData(DayOfWeek.Friday,    DateTimeExtensions.FRIDAY)]
    [InlineData(DayOfWeek.Saturday,  DateTimeExtensions.SATURDAY)]
    public void ToTimeDay_ReturnsCorrectConstant(DayOfWeek day, int expected)
    {
        Assert.Equal(expected, day.ToTimeDay());
    }

    // -------------------------------------------------------------------------
    // GetActualMaximum
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(DateTimeExtensions.SECOND,   59)]
    [InlineData(DateTimeExtensions.MINUTE,   59)]
    [InlineData(DateTimeExtensions.HOUR,     23)]
    [InlineData(DateTimeExtensions.MONTH,    11)]
    [InlineData(DateTimeExtensions.YEAR,   2037)]
    [InlineData(DateTimeExtensions.WEEK_DAY,  6)]
    public void GetActualMaximum_FixedFields_ReturnExpectedValue(int field, int expected)
    {
        var date = new DateTime(2024, 6, 15);
        Assert.Equal(expected, date.GetActualMaximum(field));
    }

    [Theory]
    [InlineData(2024,  1, 31)]
    [InlineData(2024,  2, 29)]
    [InlineData(2023,  2, 28)]
    [InlineData(2100,  2, 28)]
    [InlineData(2000,  2, 29)]
    [InlineData(2024,  4, 30)]
    [InlineData(2024, 12, 31)]
    public void GetActualMaximum_MonthDay_ReturnsCorrectDaysInMonth(int year, int month, int expected)
    {
        var date = new DateTime(year, month, 1);
        Assert.Equal(expected, date.GetActualMaximum(DateTimeExtensions.MONTH_DAY));
    }

    [Theory]
    [InlineData(2024, 365)]
    [InlineData(2023, 364)]
    public void GetActualMaximum_YearDay_ReturnsCorrectValue(int year, int expected)
    {
        var date = new DateTime(year, 1, 1);
        Assert.Equal(expected, date.GetActualMaximum(DateTimeExtensions.YEAR_DAY));
    }

    [Fact]
    public void GetActualMaximum_WeekNum_ThrowsNotSupportedException()
    {
        var date = new DateTime(2024, 1, 1);
        Assert.Throws<NotSupportedException>(() => date.GetActualMaximum(DateTimeExtensions.WEEK_NUM));
    }

    [Fact]
    public void GetActualMaximum_UnknownField_ThrowsArgumentOutOfRangeException()
    {
        var date = new DateTime(2024, 1, 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => date.GetActualMaximum(99));
    }

    // -------------------------------------------------------------------------
    // GetWeekNumber (ISO 8601) - delegates to ISOWeek.GetWeekOfYear
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData(2024,  1,  1,  1)]
    [InlineData(2024,  1,  7,  1)]
    [InlineData(2024,  1,  8,  2)]
    [InlineData(2024, 12, 30,  1)]
    [InlineData(2025,  1,  1,  1)]
    [InlineData(2020, 12, 28, 53)]
    [InlineData(2021,  1,  4,  1)]
    public void GetWeekNumber_ReturnsIso8601WeekNumber(int year, int month, int day, int expected)
    {
        var date = new DateTime(year, month, day);
        Assert.Equal(expected, date.GetWeekNumber());
    }

    // -------------------------------------------------------------------------
    // Truncate
    // -------------------------------------------------------------------------

    [Fact]
    public void Truncate_ToMinutes_ClearsSeconds()
    {
        var date = new DateTime(2024, 6, 15, 12, 30, 45, 500);
        Assert.Equal(new DateTime(2024, 6, 15, 12, 30, 0), date.Truncate(TimeSpan.FromMinutes(1)));
    }

    [Fact]
    public void Truncate_ToHours_ClearsMinutesAndBelow()
    {
        var date = new DateTime(2024, 6, 15, 12, 30, 45);
        Assert.Equal(new DateTime(2024, 6, 15, 12, 0, 0), date.Truncate(TimeSpan.FromHours(1)));
    }

    [Fact]
    public void Truncate_ZeroTimeSpan_ReturnsOriginal()
    {
        var date = new DateTime(2024, 6, 15, 12, 30, 45);
        Assert.Equal(date, date.Truncate(TimeSpan.Zero));
    }

    [Fact]
    public void Truncate_MinValue_ReturnsMinValue()
    {
        Assert.Equal(DateTime.MinValue, DateTime.MinValue.Truncate(TimeSpan.FromMinutes(1)));
    }

    [Fact]
    public void Truncate_MaxValue_ReturnsMaxValue()
    {
        Assert.Equal(DateTime.MaxValue, DateTime.MaxValue.Truncate(TimeSpan.FromMinutes(1)));
    }
}
