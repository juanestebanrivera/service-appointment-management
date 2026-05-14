using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public static class ServiceErrors
{
    public static readonly Error NameIsRequired = new(ErrorType.Validation, "Name is required");
    public static readonly Error PriceMustBeGreaterThanZero = new(ErrorType.Validation, "Price must be greater than zero");
    public static readonly Error DurationMustBeMoreThanFiveMinutes = new(ErrorType.Validation, "Duration must be more than five minutes");
    public static readonly Error DurationMustBeLessThanOneDay = new(ErrorType.Validation, "Duration must be less than one day");
    public static readonly Error ServiceIsInactive = new(ErrorType.Conflict, "The service is inactive");
}