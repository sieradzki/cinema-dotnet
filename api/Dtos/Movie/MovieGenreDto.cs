using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Movie
{
    public class MovieGenreDto
    {
        [Required(ErrorMessage = "Genre cannot be empty.")]
        public int GenreId { get; set; }
    }
}