package service

import (
	"context"
	"time"

	"github.com/google/uuid"
	"github.com/juanestebanrivera/bookings-service/internal/booking/domain"
)

type BookingRepository interface {
	Create(ctx context.Context, b *domain.Booking) error
	Search(ctx context.Context, date time.Time, serviceId uuid.UUID) ([]*domain.Booking, error)
}

type AvailabilityRepository interface {
	IsAvailable(ctx context.Context)
	Block(ctx context.Context)
	Liberate(ctx context.Context)
}

type BookingService struct {
	bookingRepository      BookingRepository
	availabilityRepository AvailabilityRepository
}

func New(br BookingRepository, ar AvailabilityRepository) *BookingService {
	return &BookingService{
		bookingRepository:      br,
		availabilityRepository: ar,
	}
}

func (s *BookingService) CreateBooking(ctx context.Context, b *domain.Booking) error {
	if err := b.Valid(); err != nil {
		return err
	}

	b.ID = uuid.New()
	return s.bookingRepository.Create(ctx, b)
}

func (s *BookingService) SearchBookings(ctx context.Context, date time.Time, serviceId uuid.UUID) ([]*domain.Booking, error) {
	return s.bookingRepository.Search(ctx, date, serviceId)
}
