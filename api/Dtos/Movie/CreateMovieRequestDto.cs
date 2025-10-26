using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Movie
{
    public class CreateMovieRequestDto
    {
        [Required(ErrorMessage = "Movie name cannot be empty.")]
        [MaxLength(256, ErrorMessage = "Movie title cannot be longer than 256 characters.")]
        public required string Title{ get; set; }
        [MaxLength(2048, ErrorMessage = "Movie description cannot be longer than 2048 characters.")]
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        [Required(ErrorMessage = "Movie duration must be specified.")]
        public int Duration { get; set; }
    }
}