package domain

import (
	"time"

	"github.com/google/uuid"
)

type BookingState int

const (
	Pending BookingState = iota
	Confirmed
	Cancel
)

type Booking struct {
	ID        uuid.UUID    `json:"id"`
	ClientID  uuid.UUID    `json:"clientId"`
	ServiceID uuid.UUID    `json:"serviceId"`
	Date      time.Time    `json:"date"`
	StartTime time.Time    `json:"startTime"`
	EndTime   time.Time    `json:"endTime"`
	State     BookingState `json:"state"`
}

func (b *Booking) Valid() error {
	if b.StartTime.Before(time.Now()) {
		return ErrStartTimeInPast
	}

	if b.EndTime.Before(b.StartTime) {
		return ErrEndTimeBeforeStartTime
	}

	return nil
}
