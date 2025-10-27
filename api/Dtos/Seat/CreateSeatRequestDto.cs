using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Validation;

namespace api.Dtos.Seat
{
    public class CreateSeatRequestDto
    {

        [Range(1, int.MaxValue, ErrorMessage = "Seat row must be positive.")]
        public int Row { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seat number must be positive.")]
        public int Number { get; set; } 
        [DefinedEnum]
        public SeatType Type { get; set; } = SeatType.Single;
    }
}