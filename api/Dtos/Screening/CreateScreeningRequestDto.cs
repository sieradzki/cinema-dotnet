using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Screening
{
    public class CreateScreeningRequestDto
    {
        [Required(ErrorMessage = "Screening start time cannot be empty.")]
        public DateTime TimeStart { get; set; }
        [Required(ErrorMessage = "Screening RoomId cannot be empty.")] // can it though?
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Screening Price cannot be empty.")]
        public decimal Price { get; set; }
    }
}