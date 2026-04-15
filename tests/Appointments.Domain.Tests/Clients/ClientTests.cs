using Appointments.Domain.Clients;

namespace Appointments.Domain.Tests.Clients;

public class ClientTests
{
    [Fact]
    public void Register_WhenDataIsValid_ReturnsSuccessAndCreatesClient()
    {
        // Arrange
        var firstName = PersonName.Create("First Name", "FirstName").Value;
        var lastName = PersonName.Create("Last Name", "LastName").Value;
        var phone = PhoneNumber.Create("+57", "1234567890").Value;
        var email = Email.Create("username@domain.com").Value;

        // Act
        var result = Client.Register(firstName, lastName, phone, email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value!.Id);
        Assert.Equal(firstName, result.Value!.FirstName);
        Assert.Equal(lastName, result.Value.LastName);
        Assert.Equal(phone, result.Value.Phone);
        Assert.Equal(email, result.Value.Email);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Register_WhenEmailIsNotProvided_ReturnsSuccessAndCreatesClient()
    {
        // Arrange
        var firstName = PersonName.Create("First Name", "FirstName").Value;
        var lastName = PersonName.Create("Last Name", "LastName").Value;
        var phone = PhoneNumber.Create("+57", "1234567890").Value;

        // Act
        var result = Client.Register(firstName, lastName, phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(firstName, result.Value.FirstName);
        Assert.Equal(lastName, result.Value.LastName);
        Assert.Equal(phone, result.Value.Phone);
        Assert.Null(result.Value.Email);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void ChangeName_WhenDataIsValid_ReturnsSuccessAndUpdatesName()
    {
        // Arrange
        var client = CreateValidClient();
        var newFirstName = PersonName.Create("New First Name", "FirstName").Value;
        var newLastName = PersonName.Create("New Last Name", "LastName").Value;

        // Act
        var result = client.ChangeName(newFirstName, newLastName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newFirstName, client.FirstName);
        Assert.Equal(newLastName, client.LastName);
    }

    [Fact]
    public void ChangeEmail_WhenEmailIsNull_UpdatesEmailToNull()
    {
        // Arrange
        var client = CreateValidClient();
        Email? newEmail = null;

        // Act
        client.ChangeEmail(newEmail);

        // Assert
        Assert.Null(client.Email);
    }

    [Fact]
    public void ChangeEmail_WhenEmailIsValid_UpdatesEmail()
    {
        // Arrange
        var client = CreateValidClient();
        var newEmail = Email.Create("newusername@domain.com").Value;

        // Act
        client.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, client.Email);
    }

    [Fact]
    public void ChangePhoneNumber_WhenPhoneNumberIsValid_UpdatesPhoneNumber()
    {
        // Arrange
        var client = CreateValidClient();
        var newPhone = PhoneNumber.Create("+57", "0987654321").Value;

        // Act
        client.ChangePhoneNumber(newPhone);

        // Assert
        Assert.Equal(newPhone, client.Phone);
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
            PersonName.Create("First Name", "FirstName").Value,
            PersonName.Create("Last Name", "LastName").Value,
            PhoneNumber.Create("+57", "1234567890").Value,
            Email.Create("username@domain.com").Value
        );

        return result.Value!;
    }
}