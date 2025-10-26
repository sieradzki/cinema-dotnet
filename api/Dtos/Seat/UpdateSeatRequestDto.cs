using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Seat
{
    public class UpdateSeatRequestDto
    {
        [Required(ErrorMessage = "Seat row cannot be empty.")]
        public int Row { get; set; }
        [Required(ErrorMessage = "Seat Number cannot be empty.")]
        public int Number { get; set; } 
        [EnumDataType(typeof(SeatType))]
        public SeatType Type { get; set; } = SeatType.Single;
    }
}