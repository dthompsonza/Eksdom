using System.Text.Json.Serialization;
using Eksdom.Shared.Serialization;
using EnsureThat;

namespace Eksdom.Shared;

/// <summary>
/// A period of time consisting of a <see cref="Start"/> and <see cref="End"/> time
/// </summary>
[Serializable]
[JsonConverter(typeof(TimePeriodJsonConverter))]
public class TimePeriod : Tuple<TimeOnly, TimeOnly>
{
    /// <summary>
    /// Creates a new <see cref="TimePeriod"/>
    /// </summary>
    public TimePeriod(TimeOnly start, TimeOnly end) 
        : base(start, end)
    {
    }

    /// <summary>
    /// Creates a new <see cref="TimePeriod"/>
    /// </summary>
    /// <param name="period">yyyy-MM-dd</param>
    public TimePeriod(string period)
        : base(Extract(period).start, Extract(period).end)
    {
    }

    public TimeOnly Start => Item1;

    public TimeOnly End => Item2;

    /// <summary>
    /// Period length in minutes
    /// </summary>
    public int Minutes => (End < Start) 
        ? GetMinutesWhenEndIsTomorrow(Start, End) 
        : GetMinutesWhenEndIsToday(Start, End);

    public override string ToString() 
        => $"{Start}-{End}";

    public static implicit operator TimeSpan(TimePeriod period)
        => TimeSpan.FromMinutes(period.Minutes);

    private static (TimeOnly start, TimeOnly end) Extract(string period)
    {
        var times = period.Split("-").ToArray();
        return (ToTimeOnly(times[0]), ToTimeOnly(times[1]));
    }

    private static TimeOnly ToTimeOnly(string time)
    {
        Ensure.That(time).HasLength(5);
        var hm = time.Split(":").Select(t => Convert.ToInt32(t)).ToArray();
        return new TimeOnly(hm[0], hm[1]);
    }

    private static int GetMinutesWhenEndIsTomorrow(TimeOnly start, TimeOnly end)
    {
        var endTime = new DateTime(2000, 1, 2, end.Hour, end.Minute, end.Second);
        var startTime = new DateTime(2000, 1, 1, start.Hour, start.Minute, start.Second);

        return Convert.ToInt32((endTime - startTime).TotalMinutes);
    }

    private static int GetMinutesWhenEndIsToday(TimeOnly start, TimeOnly end)
    {
        var endTime = new DateTime(2000, 1, 1, end.Hour, end.Minute, end.Second);
        var startTime = new DateTime(2000, 1, 1, start.Hour, start.Minute, start.Second);

        return Convert.ToInt32((endTime - startTime).TotalMinutes);
    }
}
