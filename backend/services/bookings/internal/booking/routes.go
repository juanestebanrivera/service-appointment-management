package booking

import (
	"database/sql"
	"net/http"

	"github.com/juanestebanrivera/bookings-service/internal/booking/repository"
	"github.com/juanestebanrivera/bookings-service/internal/booking/service"
	"github.com/juanestebanrivera/bookings-service/internal/booking/transport"
)

func RegisterRoutes(mux *http.ServeMux) {

	a := &sql.DB{}
	bookRepository := repository.NewPostgresBookingRepository(a)
	var b service.AvailabilityRepository

	service := service.New(bookRepository, b)
	handler := transport.New(service)

	mux.HandleFunc("POST /api/v1/bookings", handler.CreateBooking)
}
