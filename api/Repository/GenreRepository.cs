using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Genre;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateAsync(Genre genreModel)
        {
            await _context.Genres.AddAsync(genreModel);
            await _context.SaveChangesAsync();
            return genreModel;
        }

        public async Task<Genre?> DeleteAsync(int id)
        {
            var genreModel = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genreModel == null) return null;

            _context.Genres.Remove(genreModel);
            await _context.SaveChangesAsync();

            return genreModel;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<Genre?> UpdateAsync(int id, UpdateGenreRequestDto updateDto)
        {
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (existingGenre == null) return null;

            existingGenre.Name = updateDto.Name;

            await _context.SaveChangesAsync();

            return existingGenre;
        }
    }
}