using Integration.EskomSePush.Types;

namespace Integration.EskomSePush.Models.Results;

/// <summary>
/// Area Information
/// </summary>
public class AreaInformation
{
    /// <summary>
    /// Area Information
    /// </summary>
    public Info Info { get; set; }

    /// <summary>
    /// Events
    /// </summary>
    public List<Event> Events { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    public Schedule Schedule { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Info.Name} ({Info.Region})";

    /// <summary>
    /// Constructor
    /// </summary>
    public AreaInformation(Info info, List<Event> events, Schedule schedule) 
    {
        Info = info;
        Events = events;
        Schedule = schedule;
    }
}

public class Info
{
    /// <summary>
    /// Name of area
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Province of area
    /// </summary>
    public string Region { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} ({Region})";

    /// <summary>
    /// Constructor
    /// </summary>
    public Info(string name, string region)
    {
        Name = name;
        Region = region;
    }
}

/// <summary>
/// Loadshedding event
/// </summary>
public class Event
{
    /// <summary>
    /// Event start
    /// </summary>
    public DateTimeOffset Start { get; }

    /// <summary>
    /// Event end
    /// </summary>
    public DateTimeOffset End { get; }

    /// <summary>
    /// Note (stage)
    /// </summary>
    public string Note { get; }

    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel} {Start} - {End}";

    /// <summary>
    /// Constructor
    /// </summary>
    public Event(DateTimeOffset start, DateTimeOffset end, string note, int stageLevel)
    {
        Start = start;
        End = end;
        Note = note;
        StageLevel = stageLevel;
    }
}

/// <summary>
/// Loadshedding schedule
/// </summary>
public class Schedule
{
    /// <summary>
    /// Schedule days
    /// </summary>
    public List<ScheduleDay> Days { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Schedule(List<ScheduleDay> days)
    {
        Days = days;
    }
}

/// <summary>
/// Loadshedding schedule for a day
/// </summary>
public class ScheduleDay
{
    /// <summary>
    /// Schedule name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Schedule date
    /// </summary>
    public DateOnly Date { get; }

    /// <summary>
    /// Schedule stages
    /// </summary>
    public List<Stage> Stages { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} {Date:dd MMM yyyy}";

    /// <summary>
    /// Constructor
    /// </summary>
    public ScheduleDay(string name, DateOnly date, List<Stage> stages)
    {
        Name = name;
        Date = date;
        Stages = stages;
    }
}

/// <summary>
/// Loadshedding stage
/// </summary>
public class Stage
{
    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage events
    /// </summary>
    public List<TimePeriod> Events { get; }

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel} ({Events.Count} events)";

    /// <summary>
    /// Constructor
    /// </summary>
    public Stage(int stageLevel, List<TimePeriod> events)
    {
        StageLevel = stageLevel;
        Events = events;
    }
}