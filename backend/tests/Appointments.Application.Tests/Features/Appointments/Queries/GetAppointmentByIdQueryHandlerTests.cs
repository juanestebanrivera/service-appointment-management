using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetAppointmentByIdQueryHandlerTests
{
    private readonly IAppointmentQueryRepository _appointmentRepository;
    private readonly GetAppointmentByIdQueryHandler _handler;

    public GetAppointmentByIdQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentQueryRepository>();
        _handler = new GetAppointmentByIdQueryHandler(_appointmentRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetAppointmentByIdQuery(Guid.NewGuid(), Guid.NewGuid(), IsAdmin: false);

        _appointmentRepository.GetDetailByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>())
                              .Returns((AppointmentDetailResult?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentApplicationErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsNotOwnerAndNotAdmin_ReturnsForbidden()
    {
        // Arrange
        var appointment = CreateAppointmentDetailResult();
        var query = new GetAppointmentByIdQuery(appointment.Id, CurrentUserId: Guid.NewGuid(), IsAdmin: false);

        _appointmentRepository.GetDetailByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>())
                              .Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentApplicationErrors.Forbidden, result.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsAdminAndNotOwner_ReturnsSuccess()
    {
        // Arrange
        var appointment = CreateAppointmentDetailResult();
        var query = new GetAppointmentByIdQuery(appointment.Id, CurrentUserId: Guid.NewGuid(), IsAdmin: true);

        _appointmentRepository.GetDetailByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>())
                              .Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsOwner_ReturnsSuccessWithAppointmentData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var appointment = CreateAppointmentDetailResult(clientUserId: userId);
        var query = new GetAppointmentByIdQuery(appointment.Id, CurrentUserId: userId, IsAdmin: false);

        _appointmentRepository.GetDetailByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>())
                              .Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(appointment.Id, result.Value.Id);
        Assert.Equal(appointment.ClientId, result.Value.ClientId);
        Assert.Equal(appointment.ClientFirstName, result.Value.ClientFirstName);
        Assert.Equal(appointment.ClientLastName, result.Value.ClientLastName);
        Assert.Equal(appointment.ClientEmail, result.Value.ClientEmail);
        Assert.Equal(appointment.ClientPhonePrefix, result.Value.ClientPhonePrefix);
        Assert.Equal(appointment.ClientPhoneNumber, result.Value.ClientPhoneNumber);
        Assert.Equal(appointment.ServiceId, result.Value.ServiceId);
        Assert.Equal(appointment.ServiceName, result.Value.ServiceName);
        Assert.Equal(appointment.PriceAtBooking, result.Value.PriceAtBooking);
        Assert.Equal(appointment.StartTime, result.Value.StartTime);
        Assert.Equal(appointment.EndTime, result.Value.EndTime);
        Assert.Equal(appointment.Status, result.Value.Status);
    }

    private static AppointmentDetailResult CreateAppointmentDetailResult(Guid? clientUserId = null)
    {
        return new(
            Id: Guid.NewGuid(),
            PriceAtBooking: 100,
            StartTime: new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
            EndTime: new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
            Status: AppointmentStatus.Pending,
            ClientId: Guid.NewGuid(),
            ClientUserId: clientUserId ?? Guid.NewGuid(),
            ClientFirstName: "FirstName",
            ClientLastName: "LastName",
            ClientEmail: "username@domain.com",
            ClientPhonePrefix: "+1",
            ClientPhoneNumber: "123456789",
            ServiceId: Guid.NewGuid(),
            ServiceName: "Service Name"
        );
    }
}
