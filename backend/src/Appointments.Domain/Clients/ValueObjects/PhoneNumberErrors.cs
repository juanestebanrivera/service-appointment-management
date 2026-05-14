using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class PhoneNumberErrors
{
    public static readonly Error PhonePrefixRequired = new(ErrorType.Validation, "Phone number prefix is required.");
    public static readonly Error InvalidPhonePrefix = new(ErrorType.Validation, "Phone number prefix is invalid.");
    public static readonly Error PhoneNumberRequired = new(ErrorType.Validation, "Phone number is required.");
    public static readonly Error InvalidPhoneNumberFormat = new(ErrorType.Validation, "Phone number format is invalid.");
}