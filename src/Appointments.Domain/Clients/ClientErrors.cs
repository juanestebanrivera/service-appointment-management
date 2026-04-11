using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class ClientErrors
{
    public static readonly Error FirstNameIsRequired = new("Client.FirstNameRequired", "First name is required.");
    public static readonly Error FirstNameCannotContainNumbers = new("Client.FirstNameCannotContainNumbers", "First name cannot contain numbers.");
    public static readonly Error FirstNameMustBeAtLeastTwoCharacters = new("Client.FirstNameMustBeAtLeastTwoCharacters", "First name must be at least two characters long.");

    public static readonly Error LastNameIsRequired = new("Client.LastNameRequired", "Last name is required.");
    public static readonly Error LastNameCannotContainNumbers = new("Client.LastNameCannotContainNumbers", "Last name cannot contain numbers.");
    public static readonly Error LastNameMustBeAtLeastTwoCharacters = new("Client.LastNameMustBeAtLeastTwoCharacters", "Last name must be at least two characters long.");

    public static readonly Error ClientIsInactive = new("Client.ClientIsInactive", "The client is inactive.");
}