using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Screening;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class ScreeningRepository : IScreeningRepository
    {
        private readonly ApplicationDbContext _context;
        public ScreeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Screening> CreateAsync(Screening screeningModel)
        {
            await _context.Screenings.AddAsync(screeningModel);
            await _context.SaveChangesAsync();
            return screeningModel;
        }

        public async Task<Screening?> DeleteAsync(int id)
        {
            var screeningModel = await _context.Screenings.FirstOrDefaultAsync(g => g.Id == id);

            if (screeningModel == null) return null;

            _context.Screenings.Remove(screeningModel);
            await _context.SaveChangesAsync();

            return screeningModel;
        }

        public async Task<List<Screening>> GetAllAsync(ScreeningQueryObject query)
        {
            var screenings = _context.Screenings.AsQueryable();

            var start = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(query.StartDate))
                start = DateTime.Parse(query.StartDate).Date;

            var end = start.AddDays(1);
            if (!string.IsNullOrWhiteSpace(query.EndDate))
                end = DateTime.Parse(query.EndDate).Date;
            
            screenings = screenings
                .Where(s => s.TimeStart.Date >= start && s.TimeStart < end); 

            return await screenings.ToListAsync();
        }

        public async Task<List<Screening>> GetAllWithReservationsAsync(ScreeningQueryObject query) // TODO both this and room in the client fix so that we call by id and we don't need to do this cause it's not optimal
        {
            var screenings = _context.Screenings
            .Include(r => r.Reservations)
            .ThenInclude(r => r.Seats)
            .AsQueryable();

            var start = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(query.StartDate))
                start = DateTime.Parse(query.StartDate).Date;

            var end = start.AddDays(1);
            if (!string.IsNullOrWhiteSpace(query.EndDate))
                end = DateTime.Parse(query.EndDate).Date;
            
            screenings = screenings
                .Where(s => s.TimeStart.Date >= start && s.TimeStart < end); 

            return await screenings.ToListAsync();
        }


        public async Task<Screening?> GetByIdAsync(int id)
        {
            return await _context.Screenings.FindAsync(id);
        }

        public async Task<Screening?> GetByIdWithReservationsAsync(int id)
        {
            return await _context.Screenings
             .Include(r => r.Reservations)
             .ThenInclude(r => r.Seats)
             .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<bool> ScreeningExists(int id)
        {
            return _context.Screenings.AnyAsync(s => s.Id == id);
        }

        public async Task<Screening?> UpdateAsync(int id, UpdateScreeningRequestDto updateDto)
        {
            var existingScreening = await _context.Screenings.FirstOrDefaultAsync(g => g.Id == id);

            if (existingScreening == null) return null;

            existingScreening.TimeStart = updateDto.TimeStart;
            existingScreening.RoomId = updateDto.RoomId;
            existingScreening.MovieId = updateDto.MovieId;
            existingScreening.Price = updateDto.Price;

            await _context.SaveChangesAsync();

            return existingScreening;
        }
    }
}