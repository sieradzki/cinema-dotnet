using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Seat;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;
        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> CreateAsync(Seat seatModel)
        {
            await _context.Seats.AddAsync(seatModel);
            await _context.SaveChangesAsync();
            return seatModel;
        }

        public async Task<Seat?> DeleteAsync(int id)
        {
            var seatModel = await _context.Seats.FirstOrDefaultAsync(g => g.Id == id);

            if (seatModel == null) return null;

            _context.Seats.Remove(seatModel);
            await _context.SaveChangesAsync();

            return seatModel;
        }

        public async Task<List<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<Seat?> UpdateAsync(int id, Seat seatModel)
        {
            var existingSeat = await _context.Seats.FindAsync(id);

            if (existingSeat == null) return null;

            existingSeat.Row = seatModel.Row;
            existingSeat.Number = seatModel.Number;
            existingSeat.Type = seatModel.Type;

            await _context.SaveChangesAsync();

            return existingSeat;
        }
    }
}