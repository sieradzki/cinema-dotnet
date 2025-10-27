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
        [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive integer.")]
        public int RoomId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MovieId must be a positive integer.")]
        public int MovieId { get; set; }
        [Range(0.01, 9999.99, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
    }
}