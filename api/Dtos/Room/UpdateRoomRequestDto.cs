using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Room
{
    public class UpdateRoomRequestDto
    {
        [Required(ErrorMessage = "Room name cannot be empty.")]
        [MaxLength(16, ErrorMessage = "Room name cannot be longer than 16 characters.")]
        public required string Name { get; set; }
    }
}