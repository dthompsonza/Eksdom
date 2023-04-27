//namespace Eksdom.Shared;

//public static class Planner
//{

//    // returns true if stage level > 0 and current area is being loadshed
//    public static bool? IsLoadshedding(this Status status, AreaOverrides @override)
//    {
//        var statusDetail = status.GetStatusDetail(@override);

//        return statusDetail is null ? null : statusDetail.StageLevel > 0;
//    }

//    public static (DateTimeOffset Starts, TimeSpan Length)? NextOrCurrentLoadshedding(this AreaInformation areaInformation,
//            Status status,
//            AreaOverrides @override,
//            DateTimeOffset? effectiveDate = null)
//    {
//        var result = CalculateNextDate(areaInformation, status, @override, effectiveDate);
//        return result.current ?? result.next;
//    }

//    private static 
//        (
//            (DateTimeOffset, TimeSpan)? current, 
//            (DateTimeOffset, TimeSpan)? next
//        ) 
//        CalculateNextDate(
//            this AreaInformation areaInformation, 
//            Status status, 
//            AreaOverrides @override, 
//            DateTimeOffset? effectiveDate = null)
//    {
//        if (status.IsLoadshedding(@override).IsNullOrFalse())
//        {
//            return (null, null);
//        }

//        var statusDetail = status.GetStatusDetail(@override);
//        if (statusDetail is null)
//        {
//            return (null, null);
//        }

//        effectiveDate ??= DateTimeOffset.Now;

//        var currentEvent = areaInformation.Events
//            .Where(e => e.StageLevel == statusDetail.StageLevel)
//            .FirstOrDefault(e => e.Start < effectiveDate && e.End > effectiveDate);

//        (DateTimeOffset, TimeSpan)? current = null;
//        if (currentEvent is not null)
//        {
//            current = (currentEvent.Start, currentEvent.Length);
//        }
        
//        var nextEvent = areaInformation.Events
//            .Where(e => e.StageLevel == statusDetail.StageLevel)
//            .OrderBy(e => e.Start)
//            .FirstOrDefault(e => e.Start > effectiveDate && e.End > effectiveDate);

//        (DateTimeOffset, TimeSpan)? next = null;
//        if (nextEvent is not null)
//        {
//            next = (nextEvent.Start, nextEvent.Length);
//        }

//        return (current, next);
//    }

//    internal static StatusDetail GetStatusDetail(this Status status, AreaOverrides @override)
//    {
//        return @override switch
//        {
//            AreaOverrides.National => status.Eskom,
//            AreaOverrides.CapeTown => status.CapeTown,
//            _ => throw new NotImplementedException($"Unknown area override - {@override}")
//        };
//    }
//}
