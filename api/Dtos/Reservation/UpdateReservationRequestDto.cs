using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Validation;

namespace api.Dtos.Reservation
{
    public class UpdateReservationRequestDto
    {
        [Required(ErrorMessage = "Reservation UserId cannot be empty.")]
        public required string UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ScreeningId must be a positive integer.")]
        public int ScreeningId { get; set; }
        [DefinedEnum]
        public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
    }
}