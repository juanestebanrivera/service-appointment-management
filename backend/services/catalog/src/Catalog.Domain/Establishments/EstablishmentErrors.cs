using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Establishments;

public static class EstablishmentErrors
{
    public static readonly Error CommercialNameCannotBeEmpty = new(ErrorType.Validation, "Commercial name cannot be empty.");
    public static readonly Error AddressCannotBeEmpty = new(ErrorType.Validation, "Address cannot be empty.");
    public static readonly Error PhoneNumberCannotBeEmpty = new(ErrorType.Validation, "Phone number cannot be empty.");
    public static readonly Error PhoneNumberCannotContainLetters = new(ErrorType.Validation, "Phone number cannot contain letters.");

    public static readonly Error WeeklySchedulesCannotBeEmpty = new(ErrorType.Validation, "Weekly schedules cannot be empty.");
    public static readonly Error WeeklyScheduleOnlyOnePerDay = new(ErrorType.Validation, "There can only be one weekly schedule per day.");
    public static readonly Error WeeklyScheduleMaximumSevenSchedules = new(ErrorType.Validation, "There can be a maximum of seven weekly schedules.");

    public static readonly Error NotFound = new(ErrorType.NotFound, "Establishment not found.");
}