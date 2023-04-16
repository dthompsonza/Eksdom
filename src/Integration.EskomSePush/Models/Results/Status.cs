namespace Integration.EskomSePush.Models.Results;

/// <summary>
/// Current and next loadshedding status
/// </summary>
public class Status
{
    /// <summary>
    /// Eskom status
    /// </summary>
    public StatusDetail Eskom { get; }

    /// <summary>
    /// Cape Town status
    /// </summary>
    public StatusDetail CapeTown { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Status(StatusDetail eskom, 
        StatusDetail capeTown)
    {
        Eskom = eskom;
        CapeTown = capeTown;
    }
}

public class StatusDetail
{
    /// <summary>
    /// Stage name (ie 'Cape Town' or 'Eskom')
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage updated
    /// </summary>
    public DateTimeOffset Updated { get; }

    /// <summary>
    /// Next stages
    /// </summary>
    public List<StatusNextStage> NextStages { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} (Stage {StageLevel})";

    /// <summary>
    /// Constructor
    /// </summary>
    public StatusDetail(string name, 
        int stageLevel, 
        DateTimeOffset updated, 
        List<StatusNextStage> nextStages)
    {
        Name = name;
        StageLevel = stageLevel;
        Updated = updated;
        NextStages = nextStages;
    }
}

public class StatusNextStage
{
    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage starts
    /// </summary>
    public DateTimeOffset Starts { get; }

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel}";

    /// <summary>
    /// Constructor
    /// </summary>
    public StatusNextStage(int stageLevel, 
        DateTimeOffset starts)
    {
        StageLevel = stageLevel;
        Starts = starts;
    }
}

