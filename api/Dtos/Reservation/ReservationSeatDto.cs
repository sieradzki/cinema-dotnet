using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Reservation
{
    public class ReservationSeatDto
    {
        // so i'm not sure if this should be done in bulk on reservation create/update but for now i'm doing it like this cause i'd have to think :o
        [Required(ErrorMessage = "SeatId cannot be empty.")]
        public int SeatId { get; set; }
    }
}