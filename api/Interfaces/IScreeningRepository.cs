using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Screening;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IScreeningRepository
    {
        Task<List<Screening>> GetAllAsync(ScreeningQueryObject query);
        Task<List<Screening>> GetAllWithReservationsAsync(ScreeningQueryObject query);
        Task<Screening?> GetByIdAsync(int id);
        Task<Screening?> GetByIdWithReservationsAsync(int id);
        Task<Screening> CreateAsync(Screening screeningModel);
        Task<Screening?> UpdateAsync(int id, UpdateScreeningRequestDto updateDto);
        Task<Screening?> DeleteAsync(int id);
        Task<bool> ScreeningExists(int id);
    }
}