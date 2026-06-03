package transport

import (
	"encoding/json"
	"net/http"

	"github.com/juanestebanrivera/bookings-service/internal/booking/domain"
	"github.com/juanestebanrivera/bookings-service/internal/booking/service"
)

type BookingHandler struct {
	service *service.BookingService
}

func New(s *service.BookingService) *BookingHandler {
	return &BookingHandler{service: s}
}

func (h *BookingHandler) CreateBooking(w http.ResponseWriter, r *http.Request) {
	var req domain.Booking
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, "Invalid request body", http.StatusBadRequest)
		return
	}

	err := h.service.CreateBooking(r.Context(), &req)
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusCreated)
	json.NewEncoder(w).Encode(map[string]string{"id": req.ID.String()})
}
