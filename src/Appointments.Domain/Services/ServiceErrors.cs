using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public static class ServiceErrors
{
    public static readonly Error NameIsRequired = new ("Service.NameIsRequired", "Name is required");
    public static readonly Error PriceMustBeGreaterThanZero = new ("Service.PriceMustBeGreaterThanZero", "Price must be greater than zero");
    public static readonly Error DurationMustBeMoreThanFiveMinutes = new ("Service.DurationMustBeMoreThanFiveMinutes", "Duration must be more than five minutes");
    public static readonly Error DurationMustBeLessThanOneDay = new ("Service.DurationMustBeLessThanOneDay", "Duration must be less than one day");
    public static readonly Error ServiceIsInactive = new ("Service.ServiceIsInactive", "The service is inactive");
}