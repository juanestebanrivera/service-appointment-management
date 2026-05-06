using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class PhoneNumberErrors
{
    public static readonly Error PhonePrefixRequired = new("PhoneNumber.PrefixRequired", "Phone number prefix is required.", ErrorType.Validation);
    public static readonly Error InvalidPhonePrefix = new("PhoneNumber.InvalidPrefix", "Phone number prefix is invalid.", ErrorType.Validation);

    public static readonly Error PhoneNumberRequired = new("PhoneNumber.Required", "Phone number is required.", ErrorType.Validation);
    public static readonly Error InvalidPhoneNumberFormat = new("PhoneNumber.InvalidFormat", "Phone number format is invalid.", ErrorType.Validation);
}