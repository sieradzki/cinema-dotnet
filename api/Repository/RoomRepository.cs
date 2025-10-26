using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Room;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room> CreateAsync(Room roomModel)
        {
            await _context.Rooms.AddAsync(roomModel);
            await _context.SaveChangesAsync();
            return roomModel;
        }

        public async Task<Room?> DeleteAsync(int id)
        {
            var roomModel = await _context.Rooms.FirstOrDefaultAsync(g => g.Id == id);

            if (roomModel == null) return null;

            _context.Rooms.Remove(roomModel);
            await _context.SaveChangesAsync();

            return roomModel;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<List<Room>> GetAllWithSeatsAsync()
        {
            return await _context.Rooms.Include(s => s.Seats).ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Room?> GetByIdWithSeatsAsync(int id)
        {
            return await _context.Rooms.Include(s => s.Seats).FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<bool> RoomExists(int id)
        {
            return _context.Rooms.AnyAsync(r => r.Id == id);
        }

        public async Task<Room?> UpdateAsync(int id, UpdateRoomRequestDto updateDto)
        {
            var existingRoom = await _context.Rooms.FirstOrDefaultAsync(g => g.Id == id);

            if (existingRoom == null) return null;

            existingRoom.Name = updateDto.Name;

            await _context.SaveChangesAsync();

            return existingRoom;
        }
    }
}