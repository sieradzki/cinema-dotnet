using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Movie;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateAsync(Movie movieModel)
        {
            await _context.Movies.AddAsync(movieModel);
            await _context.SaveChangesAsync();
            return movieModel;
        }

        public async Task<Movie?> DeleteAsync(int id)
        {
            var movieModel = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movieModel == null) return null;

            _context.Movies.Remove(movieModel);
            await _context.SaveChangesAsync();

            return movieModel;
        }

        public async Task<List<Movie>> GetAllAsync(MovieQueryObject query)
        {
            var movies = _context.Movies
                .Include(g => g.Genres)
                // .Include(s => s.Screenings) let's not include screenings
                .AsQueryable();

            // filters
            if (!string.IsNullOrWhiteSpace(query.GenreName))
                movies = movies.Where(m => m.Genres.Any(g => g.Name == query.GenreName));
            if (!string.IsNullOrWhiteSpace(query.Title))
                movies = movies.Where(m => m.Title.ToLower().Contains(query.Title.ToLower()));

            // sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    movies = query.IsDescending
                    ? movies.OrderByDescending(m => m.Title)
                    : movies.OrderBy(m => m.Title);
                }
            }

            return await movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .Include(g => g.Genres)
                .Include(s => s.Screenings)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<bool> MovieExists(int id)
        {
            return _context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task<Movie?> UpdateAsync(int id, UpdateMovieRequestDto updateDto)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null) return null;

            existingMovie.Title = updateDto.Title;
            existingMovie.Description = updateDto.Description;
            existingMovie.PosterUrl = updateDto.PosterUrl;
            existingMovie.Duration = updateDto.Duration;

            await _context.SaveChangesAsync();

            return existingMovie;
        }
    }
}