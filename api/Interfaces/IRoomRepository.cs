using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Room;
using api.Models;

namespace api.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync();
        Task<List<Room>> GetAllWithSeatsAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<Room?> GetByIdWithSeatsAsync(int id);
        Task<Room> CreateAsync(Room roomModel);
        Task<Room?> UpdateAsync(int id, UpdateRoomRequestDto updateDto);
        Task<Room?> DeleteAsync(int id);
        Task<bool> RoomExists(int id);
    }
}