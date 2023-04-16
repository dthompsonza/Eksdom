using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;
using Integration.EskomSePush.Models.Responses;
using Integration.EskomSePush.Models.Results;
using Integration.EskomSePush.Types;

namespace Integration.EskomSePush
{
    internal static class DtoToResultsMapper
    {
        #region Check allowance

        public static Allowance Map(this AllowanceResponse allowanceResponse)
        {
            ValidateModel(allowanceResponse);

            return new Allowance(count: allowanceResponse.Allowance.Count, 
                limit: allowanceResponse.Allowance.Limit);
        }

        #endregion

        #region Status

        public static Status Map(this StatusResponse statusResponse)
        {
            ValidateModel(statusResponse);

            return new Status(
                eskom: Map(statusResponse.Eskom),
                capeTown: Map(statusResponse.CapeTown));
        }

        private static StatusDetail Map(this StatusDetailDto statusDetailDto)
        {
            ValidateModel(statusDetailDto);

            return new(
                name: statusDetailDto.Name!, 
                stageLevel: statusDetailDto.Stage!.Value, 
                updated: statusDetailDto.Updated!.Value, 
                nextStages: statusDetailDto.NextStages.SelectList(Map));
        }

        private static StatusNextStage Map(this StatusNextStageDto nextStageDto)
        {
            ValidateModel(nextStageDto);

            return new StatusNextStage(nextStageDto.Stage!.Value, nextStageDto.Starts!.Value);
        }

        #endregion

        #region Area Information

        public static AreaInformation Map(this AreaInformationResponse areaInformationResponse)
        {
            ValidateModel(areaInformationResponse);

            return new AreaInformation(
                info: Map(areaInformationResponse.Info), 
                events: areaInformationResponse.Events.SelectList(Map), 
                schedule: Map(areaInformationResponse.Schedule));
        }

        private static Info Map(InfoDto infoDto)
        {
            ValidateModel(infoDto);

            return new Info(infoDto.Name!, infoDto.Region!);
        }

        private static Event Map(EventDto eventDto)
        {
            ValidateModel(eventDto);

            var start = DateTimeOffset.Parse(eventDto.Start!, CultureInfo.InvariantCulture);
            var end = DateTimeOffset.Parse(eventDto.End!, CultureInfo.InvariantCulture);
            return new Event(start, end, eventDto.Note!, ExtractStageLevel(eventDto.Note!));
        }

        private static int ExtractStageLevel(string note)
        {
            var s = note.Split(" ");
            var level = Convert.ToInt32(s[1]);
            return level;
        }

        private static Schedule Map(ScheduleDto scheduleDto)
        {
            ValidateModel(scheduleDto);

            return new Schedule(scheduleDto.Days.SelectList(Map));
        }

        private static ScheduleDay Map(ScheduleDayDto scheduleDayDto)
        {
            ValidateModel(scheduleDayDto);

            var date = DateOnly.ParseExact(scheduleDayDto.Date!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var eventsPerStageList = scheduleDayDto.Stages.SelectList(Map);
            var stages = new List<Stage>();

            for (var stageIndex = 1; stageIndex < eventsPerStageList.Count; stageIndex++)
            {
                var events = eventsPerStageList.ElementAt(stageIndex);
                var stage = new Stage(stageIndex + 1, events);
                stages.Add(stage);
            }
            
            return new ScheduleDay(scheduleDayDto.Name!, date, stages);
        }

        private static List<TimePeriod> Map(List<string> events)
        {
            return events.SelectList(@event => new TimePeriod(@event));
        }

        #endregion

        private static bool ValidateModel(object model, [CallerArgumentExpression("model")] string? modelArgumentName = null)
        {
            if (model == null)
            {
                throw new EksdomException($"Could not validate model '{modelArgumentName}' as it was NULL");
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                var errorMessages = string.Join(Environment.NewLine, results.Select((r, i) => $"{i} - {r.ErrorMessage}"));
                throw new EksdomException($"The following errors occurred while validating response model '{modelArgumentName}'" +
                    $":{Environment.NewLine}{errorMessages}");
            }

            return true;
        }
    }
}
