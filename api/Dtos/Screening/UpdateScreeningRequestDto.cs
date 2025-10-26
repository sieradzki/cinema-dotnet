using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Screening
{
    public class UpdateScreeningRequestDto
    {
        [Required(ErrorMessage = "Screening start time cannot be empty.")]
        public DateTime TimeStart { get; set; }
        [Required(ErrorMessage = "Screening RoomId cannot be empty.")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Screening MovieId cannot be empty.")]
        public int MovieId { get; set; }
        [Required(ErrorMessage = "Screening Price cannot be empty.")]
        public decimal Price { get; set; }
    }
}