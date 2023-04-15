namespace Integration.EskomSePush.Models.Responses;

/// <summary>
/// Response model for 'area'
/// </summary>
public class AreaInformation : Response
{
    /// <summary>
    /// 
    /// </summary>
    public List<Event> Events { get; set; } = new List<Event>();

    /// <summary>
    /// 
    /// </summary>
    public Info Info { get; set; } = new Info();

    /// <summary>
    /// 
    /// </summary>
    public Schedule Schedule { get; set; } = new Schedule();
}

public class Info
{
    /// <summary>
    /// Name of area
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Province of area
    /// </summary>
    public string? Region { get; set; }
}

/// <summary>
/// Loadshedding event
/// </summary>
public class Event
{
    public string? End { get; set; }

    public string? Note { get; set; }

    public string? Start { get; set; }
}

/// <summary>
/// Loadshedding schedule
/// </summary>
public class Schedule
{
    public List<ScheduleDay> Days { get; set; } = new List<ScheduleDay>();
}

/// <summary>
/// Loadshedding schedule for a day
/// </summary>
public class ScheduleDay
{
    public string? Date { get; set; }

    public string? Name { get; set; }

    public List<List<string>> Stages { get; set; } = new List<List<string>>();
}