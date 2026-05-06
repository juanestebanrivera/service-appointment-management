using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public static class ServiceErrors
{
    public static readonly Error NameIsRequired = new("Service.NameIsRequired", "Name is required", ErrorType.Validation);
    public static readonly Error PriceMustBeGreaterThanZero = new("Service.PriceMustBeGreaterThanZero", "Price must be greater than zero", ErrorType.Validation);
    public static readonly Error DurationMustBeMoreThanFiveMinutes = new("Service.DurationMustBeMoreThanFiveMinutes", "Duration must be more than five minutes", ErrorType.Validation);
    public static readonly Error DurationMustBeLessThanOneDay = new("Service.DurationMustBeLessThanOneDay", "Duration must be less than one day", ErrorType.Validation);
    public static readonly Error ServiceIsInactive = new("Service.ServiceIsInactive", "The service is inactive", ErrorType.Conflict);
}