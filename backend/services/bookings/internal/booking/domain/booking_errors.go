package domain

import "errors"

var (
	ErrStartTimeInPast        = errors.New("start time cannot be in the past")
	ErrEndTimeBeforeStartTime = errors.New("end time cannot be before start time")

	ErrBookingNotFound = errors.New("booking not found")
)
