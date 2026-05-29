using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Establishments;

public class Establishment
{
    public Guid Id { get; private set; }
    public string CommercialName { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public IReadOnlyCollection<WeeklySchedule> WeeklySchedules => _weeklySchedules.AsReadOnly();
    private List<WeeklySchedule> _weeklySchedules { get; set; } = [];

    private Establishment() { }

    public Result UpdateBasicInfo(string commercialName, string address, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(commercialName))
            return Result.Failure(EstablishmentErrors.CommercialNameCannotBeEmpty);

        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure(EstablishmentErrors.AddressCannotBeEmpty);

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result.Failure(EstablishmentErrors.PhoneNumberCannotBeEmpty);

        if (phoneNumber.Any(char.IsLetter))
            return Result.Failure(EstablishmentErrors.PhoneNumberCannotContainLetters);

        CommercialName = commercialName;
        Address = address;
        PhoneNumber = phoneNumber;

        return Result.Success();
    }

    public Result UpdateWeeklySchedules(List<WeeklySchedule> weeklySchedules)
    {
        if (weeklySchedules == null || weeklySchedules.Count == 0)
            return Result.Failure(EstablishmentErrors.WeeklySchedulesCannotBeEmpty);

        if (weeklySchedules.Count > 7)
            return Result.Failure(EstablishmentErrors.WeeklyScheduleMaximumSevenSchedules);

        var groupedByDay = weeklySchedules.GroupBy(ws => ws.DayOfWeek);
        if (groupedByDay.Any(g => g.Count() > 1))
            return Result.Failure(EstablishmentErrors.WeeklyScheduleOnlyOnePerDay);

        _weeklySchedules = weeklySchedules.ToList();
        return Result.Success();
    }   
}