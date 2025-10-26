using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Movie;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync(MovieQueryObject query);
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie> CreateAsync(Movie movieModel);
        Task<Movie?> UpdateAsync(int id, UpdateMovieRequestDto updateDto);
        Task<Movie?> DeleteAsync(int id);
        Task<bool> MovieExists(int id);
    }
}