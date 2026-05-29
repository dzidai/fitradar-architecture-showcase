using Fitradar.SharedKernel.Extensions;

namespace Fitradar.SharedKernel.Tests.Extensions;

public class UnixTimeConverterTests
{
    [Fact]
    public void ToUnixTimeMilliseconds_UnixEpoch_ReturnsZero()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Assert.Equal(0L, epoch.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void RoundTrip_PreservesUtcDateTime()
    {
        var original = new DateTime(2024, 6, 15, 12, 30, 45, DateTimeKind.Utc);
        long millis = original.ToUnixTimeMilliseconds();
        DateTime restored = UnixTimeConverter.FromUnixTimeMilliseconds(millis);

        Assert.Equal(original, restored);
    }

    [Fact]
    public void FromUnixTimeMilliseconds_ReturnsUtcKind()
    {
        DateTime result = UnixTimeConverter.FromUnixTimeMilliseconds(0);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Theory]
    [InlineData(0L,             1970, 1, 1,  0,  0,  0)]
    [InlineData(1_000L,         1970, 1, 1,  0,  0,  1)]
    [InlineData(86_400_000L,    1970, 1, 2,  0,  0,  0)]
    [InlineData(1_718_454_645_000L, 2024, 6, 15, 12, 30, 45)]
    public void FromUnixTimeMilliseconds_KnownValues(long ms, int year, int month, int day, int hour, int minute, int second)
    {
        DateTime result = UnixTimeConverter.FromUnixTimeMilliseconds(ms);

        Assert.Equal(year,   result.Year);
        Assert.Equal(month,  result.Month);
        Assert.Equal(day,    result.Day);
        Assert.Equal(hour,   result.Hour);
        Assert.Equal(minute, result.Minute);
        Assert.Equal(second, result.Second);
    }
}
