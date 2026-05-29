using Fitradar.SharedKernel.Extensions;

namespace Fitradar.SharedKernel.Tests.Extensions;

public class Rfc2445FormatterTests
{
    // -------------------------------------------------------------------------
    // ToRfc2445String
    // -------------------------------------------------------------------------

    [Fact]
    public void ToRfc2445String_FormatsCorrectly()
    {
        var date = new DateTime(2024, 6, 15, 12, 30, 45);
        Assert.Equal("20240615T123045", date.ToRfc2445String());
    }

    // -------------------------------------------------------------------------
    // ParseFromRfc2445 - valid inputs
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("20240615",         2024, 6, 15,  0,  0,  0)]
    [InlineData("20240615T123045",  2024, 6, 15, 12, 30, 45)]
    [InlineData("20240615T123045Z", 2024, 6, 15, 12, 30, 45)]
    public void ParseFromRfc2445_ValidString_ReturnsCorrectDateTime(
        string input, int year, int month, int day, int hour, int minute, int second)
    {
        DateTime result = Rfc2445Formatter.ParseFromRfc2445(input);

        Assert.Equal(year,   result.Year);
        Assert.Equal(month,  result.Month);
        Assert.Equal(day,    result.Day);
        Assert.Equal(hour,   result.Hour);
        Assert.Equal(minute, result.Minute);
        Assert.Equal(second, result.Second);
    }

    [Fact]
    public void RoundTrip_PreservesUtcDateTime()
    {
        var original = new DateTime(2024, 6, 15, 12, 30, 45, DateTimeKind.Utc);
        string formatted = original.ToRfc2445String() + "Z";
        DateTime restored = Rfc2445Formatter.ParseFromRfc2445(formatted);

        Assert.Equal(original, restored);
    }

    // -------------------------------------------------------------------------
    // ParseFromRfc2445 - invalid inputs
    // -------------------------------------------------------------------------

    [Theory]
    [InlineData("2024")]              // too short (< 8)
    [InlineData("202406150")]         // length 9 — between 8 and 15
    [InlineData("20240615X123045")]   // no T at pos 8
    public void ParseFromRfc2445_InvalidString_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => Rfc2445Formatter.ParseFromRfc2445(input));
    }
}
