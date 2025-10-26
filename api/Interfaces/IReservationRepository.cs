using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Reservation;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task<List<Reservation>> GetByUserIdAsync(ReservationQueryObject query);
        Task<Reservation> CreateAsync(Reservation reservationModel);
        Task<Reservation?> UpdateAsync(int id, UpdateReservationRequestDto updateDto);
        Task<Reservation?> DeleteAsync(int id);
        Task<bool> ReservationExists(int id);
    }
}