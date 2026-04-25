using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Commands;

public class DeleteClientCommandHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteClientCommandHandler _handler;

    public DeleteClientCommandHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteClientCommandHandler(_clientRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteClientCommand(Guid.NewGuid());
        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.NotFound, result.Error);

        _clientRepository.DidNotReceive().Delete(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenClientExists_ReturnsSuccessAndDeletesClient()
    {
        // Arrange
        var client = Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            Email.Create("username@domain.com").Value
        ).Value;

        var command = new DeleteClientCommand(client.Id);

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _clientRepository.Received(1).Delete(client);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
