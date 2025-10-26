using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Genre
{
    public class GenreDto
    {
        public int Id { get; set; }
        // [Required(ErrorMessage = "You sohuld provide a name value.")]
        // [MaxLength(16, ErrorMessage = "Cannot be longer than 16 characters!")]
        public required string Name { get; set; }
    }
}