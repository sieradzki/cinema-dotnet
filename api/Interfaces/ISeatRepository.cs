using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Seat;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllAsync();
        Task<Seat?> GetByIdAsync(int id);
        Task<Seat> CreateAsync(Seat seatModel);
        Task<Seat?> UpdateAsync(int id, Seat seatModel);
        Task<Seat?> DeleteAsync(int id);
    }
}