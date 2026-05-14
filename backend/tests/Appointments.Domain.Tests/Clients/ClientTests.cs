using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;

namespace Appointments.Domain.Tests.Clients;

public class ClientTests
{
    [Fact]
    public void Register_WhenDataIsValid_ReturnsSuccessAndCreatesClient()
    {
        // Arrange
        var firstName = PersonName.Create("First Name", nameof(Client.FirstName)).Value;
        var lastName = PersonName.Create("Last Name", nameof(Client.LastName)).Value;
        var phone = PhoneNumber.Create("+57", "1234567890").Value;
        var email = Email.Create("username@domain.com").Value;
        var userId = Guid.NewGuid();

        // Act
        var result = Client.Register(firstName, lastName, phone, userId, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value!.Id);
        Assert.Equal(firstName, result.Value!.FirstName);
        Assert.Equal(lastName, result.Value.LastName);
        Assert.Equal(phone, result.Value.Phone);
        Assert.Equal(email, result.Value.Email);
        Assert.True(result.Value.IsActive);
        Assert.Equal(userId, result.Value.UserId);
    }

    [Fact]
    public void Register_WhenEmailIsNotProvided_ReturnsSuccessAndCreatesClient()
    {
        // Arrange
        var firstName = PersonName.Create("First Name", nameof(Client.FirstName)).Value;
        var lastName = PersonName.Create("Last Name", nameof(Client.LastName)).Value;
        var phone = PhoneNumber.Create("+57", "1234567890").Value;
        var userId = Guid.NewGuid();

        // Act
        var result = Client.Register(firstName, lastName, phone, userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(firstName, result.Value.FirstName);
        Assert.Equal(lastName, result.Value.LastName);
        Assert.Equal(phone, result.Value.Phone);
        Assert.Null(result.Value.Email);
        Assert.True(result.Value.IsActive);
        Assert.Equal(userId, result.Value.UserId);
    }

    [Fact]
    public void UpdateContactInfo_WhenEmailIsProvided_UpdatesAllContactInfo()
    {
        // Arrange
        var client = CreateValidClient();
        var newFirstName = PersonName.Create("New First Name", nameof(Client.FirstName)).Value;
        var newLastName = PersonName.Create("New Last Name", nameof(Client.LastName)).Value;
        var newEmail = Email.Create("newusername@domain.com").Value;
        var newPhone = PhoneNumber.Create("+57", "0987654321").Value;

        // Act
        client.UpdateContactInfo(newFirstName, newLastName, newEmail, newPhone);

        // Assert
        Assert.Equal(newFirstName, client.FirstName);
        Assert.Equal(newLastName, client.LastName);
        Assert.Equal(newEmail, client.Email);
        Assert.Equal(newPhone, client.Phone);
    }

    [Fact]
    public void UpdateContactInfo_WhenEmailIsNull_ClearsEmail()
    {
        // Arrange
        var client = CreateValidClient();
        var newFirstName = PersonName.Create("New First Name", nameof(Client.FirstName)).Value;
        var newLastName = PersonName.Create("New Last Name", nameof(Client.LastName)).Value;
        var newPhone = PhoneNumber.Create("+57", "0987654321").Value;
        Email? newEmail = null;

        // Act
        client.UpdateContactInfo(newFirstName, newLastName, newEmail, newPhone);

        // Assert
        Assert.Equal(newFirstName, client.FirstName);
        Assert.Equal(newLastName, client.LastName);
        Assert.Null(client.Email);
        Assert.Equal(newPhone, client.Phone);
    }

    [Fact]
    public void UpdateContactInfo_WhenCalled_MaintainsUserId()
    {
        // Arrange
        var client = CreateValidClient();
        var originalUserId = client.UserId;

        var newFirstName = PersonName.Create("New First", nameof(Client.FirstName)).Value;
        var newLastName = PersonName.Create("New Last", nameof(Client.LastName)).Value;
        var newPhone = PhoneNumber.Create("+57", "0987654321").Value;

        // Act
        client.UpdateContactInfo(newFirstName, newLastName, null, newPhone);

        // Assert
        Assert.Equal(originalUserId, client.UserId);
    }

    [Fact]
    public void UpdateContactInfo_WhenCalled_MaintainsIsActiveState()
    {
        // Arrange
        var client = CreateValidClient();
        client.Deactivate();

        var newFirstName = PersonName.Create("New First", nameof(Client.FirstName)).Value;
        var newLastName = PersonName.Create("New Last", nameof(Client.LastName)).Value;
        var newPhone = PhoneNumber.Create("+57", "0987654321").Value;

        // Act
        client.UpdateContactInfo(newFirstName, newLastName, null, newPhone);

        // Assert
        Assert.False(client.IsActive);
    }

    [Fact]
    public void Activate_WhenClientIsInactive_SetsIsActiveToTrue()
    {
        // Arrange
        var client = CreateValidClient();
        client.Deactivate();

        // Act
        client.Activate();

        // Assert
        Assert.True(client.IsActive);
    }

    [Fact]
    public void Activate_WhenClientIsAlreadyActive_KeepsActivated()
    {
        // Arrange
        var client = CreateValidClient();

        // Act
        client.Activate();

        // Assert
        Assert.True(client.IsActive);
    }

    [Fact]
    public void Deactivate_WhenClientIsActive_SetsIsActiveToFalse()
    {
        // Arrange
        var client = CreateValidClient();

        // Act
        client.Deactivate();

        // Assert
        Assert.False(client.IsActive);
    }

    [Fact]
    public void Deactivate_WhenClientIsAlreadyInactive_KeepsDeactivated()
    {
        // Arrange
        var client = CreateValidClient();
        client.Deactivate();

        // Act
        client.Deactivate();

        // Assert
        Assert.False(client.IsActive);
    }

    private static Client CreateValidClient()
    {
        var result = Client.Register(
            PersonName.Create("First Name", nameof(Client.FirstName)).Value,
            PersonName.Create("Last Name", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+57", "1234567890").Value,
            userId: Guid.NewGuid(),
            Email.Create("username@domain.com").Value
        );

        return result.Value!;
    }
}