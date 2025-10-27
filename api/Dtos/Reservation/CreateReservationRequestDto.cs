using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Validation;

namespace api.Dtos.Reservation
{
    public class CreateReservationRequestDto
    {
        [Required(ErrorMessage = "Reservation UserId cannot be empty.")]
        public required string UserId { get; set; }
        [DefinedEnum]
        public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
    }
}