using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Seat;
using api.Models;

namespace api.Dtos.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int ScreeningId { get; set; }
        public ReservationStatus Status { get; set; }
        public List<SeatDto> Seats { get; set; } = new();
    }
}