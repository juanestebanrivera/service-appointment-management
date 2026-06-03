package domain

import "time"

type SlotAvailability struct {
	StartTime   time.Time `json:"startTime"`
	EndTime     time.Time `json:"endTime"`
	IsAvailable bool      `json:"isAvailable"`
}
