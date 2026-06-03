package repository

import (
	"context"
	"database/sql"
	"time"

	"github.com/google/uuid"
	"github.com/juanestebanrivera/bookings-service/internal/booking/domain"
	"github.com/juanestebanrivera/bookings-service/internal/booking/service"
)

type postgresBookingRepository struct {
	db *sql.DB
}

func NewPostgresBookingRepository(db *sql.DB) service.BookingRepository {
	return &postgresBookingRepository{db: db}
}

func (r *postgresBookingRepository) Create(ctx context.Context, b *domain.Booking) error {
	q := `INSERT INTO bookings (id, clientId, serviceId, date, startTime, endTime, state)
		  VALUES ($1, $2, $3, $4, $5, $6, $7)`

	_, err := r.db.ExecContext(ctx, q, b.ID, b.ClientID, b.ServiceID, b.Date, b.StartTime, b.EndTime, b.State)
	if err != nil {
		return err
	}

	return nil
}

func (r *postgresBookingRepository) Search(ctx context.Context, date time.Time, serviceId uuid.UUID) ([]*domain.Booking, error) {
	q := `SELECT id, clientId, serviceId, date, startTime, endTime, state
			FROM bookings
			WHERE serviceId = $1 AND date = $2`

	rows, err := r.db.QueryContext(ctx, q, serviceId, date)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	var bookings []*domain.Booking
	for rows.Next() {
		var b domain.Booking
		if err := rows.Scan(&b.ID, &b.ClientID, &b.ServiceID, &b.Date, &b.StartTime, &b.EndTime, &b.State); err != nil {
			return nil, err
		}

		bookings = append(bookings, &b)
	}

	return bookings, nil
}
