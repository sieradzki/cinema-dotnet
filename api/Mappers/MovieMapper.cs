using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Genre;
using api.Dtos.Movie;
using api.Models;

namespace api.Mappers
{
    public static class MovieMapper
    {
        public static MovieDto ToMovieDto(this Movie movieModel)
        {
            return new MovieDto
            {
                Id = movieModel.Id,
                Title = movieModel.Title,
                Description = movieModel.Description,
                PosterUrl = movieModel.PosterUrl,
                Duration = movieModel.Duration,
                Genres = movieModel.Genres.Select(g => g.ToGenreDto()).ToList(),
                Screenings = movieModel.Screenings.Select(s => s.ToScreeningDto()).ToList()
            };
        }

        public static Movie ToMovieFromCreateDto(this CreateMovieRequestDto movieDto)
        {
            return new Movie
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                PosterUrl = movieDto.PosterUrl,
                Duration = movieDto.Duration
            };
        }
    }
}