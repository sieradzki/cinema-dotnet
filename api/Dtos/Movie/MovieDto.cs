using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Screening;

namespace api.Dtos.Genre
{
    public class MovieDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        public int Duration { get; set; }
        public List<GenreDto>? Genres { get; set; }
        public List<ScreeningDto>? Screenings { get; set; }
    }
}