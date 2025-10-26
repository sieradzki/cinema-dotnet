using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Reservation;

namespace api.Dtos.Screening
{
    public class ScreeningDto
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public int RoomId { get; set; }
        public int MovieId { get; set; }
        public decimal Price { get; set; }
    }

    public class ScreeningWithReservationsDto : ScreeningDto
    {
        public List<ReservationDto>? Reservations { get; set; } 
    }
}