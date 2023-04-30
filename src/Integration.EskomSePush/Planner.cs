using Eksdom.Shared;
using Microsoft.Extensions.Logging;

namespace Eksdom.Client;

public static class Planner
{
    public static Event? NextOrCurrentLoadshedding(this AreaInformation areaInformation,
            AreaOverrides @override,
            DateTimeOffset? effectiveDate = null)
    {
        var (current, next) = areaInformation.CalculateNextDate(@override, effectiveDate);

        return current ?? next ?? default;
    }

    private static
        (
            Event? current,
            Event? next
        )
        CalculateNextDate(
            this AreaInformation areaInformation,
            AreaOverrides @override,
            DateTimeOffset? effectiveDate = null)
    {
        effectiveDate ??= DateTimeOffset.Now;

        var currentEvent = areaInformation.Events
            .SingleOrDefault(e => e.Start <= effectiveDate && e.End >= effectiveDate);

        var nextEvent = areaInformation.Events
            .OrderBy(e => e.Start)
            .FirstOrDefault(e => e.Start > effectiveDate && e.End > effectiveDate);

        return (currentEvent, nextEvent);
    }

    internal static StatusDetail GetStatusDetail(this Status status, AreaOverrides @override)
    {
        return @override switch
        {
            AreaOverrides.National => status.Eskom,
            AreaOverrides.CapeTown => status.CapeTown,
            _ => throw new NotImplementedException($"Unknown area override - {@override}")
        };
    }
}
