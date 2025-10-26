using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Genre;
using api.Models;
using Npgsql.Replication;

namespace api.Mappers
{
    public static class GenreMapper
    {
        public static GenreDto ToGenreDto(this Genre genreModel)
        {
            return new GenreDto
            {
                Id = genreModel.Id,
                Name = genreModel.Name
            };
        }

        public static Genre ToGenreFromCreateDto(this CreateGenreRequestDto genreDto)
        {
            return new Genre
            {
                Name = genreDto.Name
            };
        }
    }
}