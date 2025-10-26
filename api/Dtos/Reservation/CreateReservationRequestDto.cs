using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Reservation
{
    public class CreateReservationRequestDto
    {
        [Required(ErrorMessage = "Reservation UserId cannot be empty.")]
        public required string UserId { get; set; }
        [EnumDataType(typeof(ReservationStatus))] // error?
        public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
    }
}