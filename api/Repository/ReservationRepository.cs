using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Reservation;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;
        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> CreateAsync(Reservation reservationModel)
        {
            await _context.Reservations.AddAsync(reservationModel);
            await _context.SaveChangesAsync();
            return reservationModel;
        }

        public async Task<Reservation?> DeleteAsync(int id)
        {
            var reservationModel = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            if (reservationModel == null) return null;

            _context.Reservations.Remove(reservationModel);
            await _context.SaveChangesAsync();

            return reservationModel;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(s => s.Seats)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reservation>> GetByUserIdAsync(ReservationQueryObject query)
        {
            var reservations = _context.Reservations
                .Include(s => s.Seats)
                .Where(r => r.UserId == query.userId);

            return await reservations.ToListAsync();
        }

        public Task<bool> ReservationExists(int id)
        {
            return _context.Reservations.AnyAsync(r => r.Id == id);
        }

        public async Task<Reservation?> UpdateAsync(int id, UpdateReservationRequestDto updateDto)
        {
            var existingReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            if (existingReservation == null) return null;

            existingReservation.UserId = updateDto.UserId;
            existingReservation.ScreeningId = updateDto.ScreeningId;
            existingReservation.Status = updateDto.Status;

            await _context.SaveChangesAsync();

            return existingReservation;
        }
    }
}