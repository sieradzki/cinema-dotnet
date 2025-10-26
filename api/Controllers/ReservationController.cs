using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Reservation;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationRepository _reservationRepo;
        private readonly IScreeningRepository _screeningRepo;
        public ReservationController(
            ApplicationDbContext context,
            IReservationRepository reservationRepo,
            IScreeningRepository screeningRepo
        )
        {
            _reservationRepo = reservationRepo;
            _screeningRepo = screeningRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationRepo.GetAllAsync();
            var reservationDto = reservations.Select(r => r.ToReservationDto());

            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);

            if (reservation == null) return NotFound();

            return Ok(reservation.ToReservationDto());
        }

        [HttpGet("by-user")] // this is ass xdddd
        public async Task<IActionResult> GetByUserId([FromQuery] ReservationQueryObject query)
        {
            var reservations = await _reservationRepo.GetByUserIdAsync(query);

            if (reservations == null) return NotFound();

            return Ok(reservations);
        }

        [HttpPost("{screeningId:int}")]
        public async Task<IActionResult> Create([FromRoute] int screeningId, [FromBody] CreateReservationRequestDto reservationDto)
        {
            // Check if screening exists
            if (!await _screeningRepo.ScreeningExists(screeningId))
                return BadRequest("Screening does not exist!");
            var reservationModel = reservationDto.ToReservationFromCreateDto(screeningId);

            await _reservationRepo.CreateAsync(reservationModel);

            return CreatedAtAction(nameof(GetById), new { id = reservationModel.Id }, reservationModel.ToReservationDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReservationRequestDto updateDto)
        {
            var reservationModel = await _reservationRepo.UpdateAsync(id, updateDto);
            if (reservationModel == null) return NotFound();

            return Ok(reservationModel.ToReservationDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var reservationModel = await _reservationRepo.DeleteAsync(id);
            if (reservationModel == null) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReservation([FromRoute] int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(r => r.Id == id);

            // Check if reservation exists
            if (reservation == null) return NotFound("Reservation not found.");

            // Check if reservation is already cancelled
            if (reservation.Status == ReservationStatus.Cancelled)
                return BadRequest("Reservation is already cancelled.");

            // Only allow cancelling more than 30 minutes before the screening
            var screeningStart = reservation.Screening.TimeStart;
            if (screeningStart <= DateTime.UtcNow.AddMinutes(30))
                return BadRequest("Too late to cancel reservation!");

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpPut("{id}/paid")]
        public async Task<IActionResult> PaidReservation([FromRoute] int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(r => r.Id == id);

            // Check if reservation exists
            if (reservation == null) return NotFound("Reservation not found.");

            // Check if reservation is cancelled
            if (reservation.Status == ReservationStatus.Cancelled)
                return BadRequest("Reservation is cancelled.");

            // Check if reservation is already paid
            if (reservation.Status == ReservationStatus.Paid)
                return BadRequest("Reservation is already paid.");

            reservation.Status = ReservationStatus.Paid;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("{id:int}/reserve-seat")]
        public async Task<IActionResult> ReserveSeat([FromRoute] int id, [FromBody] ReservationSeatDto dto)
        {
            // Check if reservation exists
            var reservationModel = await _context.Reservations
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reservationModel == null) return NotFound("Reservation not found.");

            // Check if seat exists
            var seat = await _context.Seats.FindAsync(dto.SeatId);
            if (seat == null) return NotFound("Seat not found.");

            // Check if seat is already reserved
            // var seatReserved = await _context.Reservations
            //     .AnyAsync(r => r.ScreeningId == screeningId && r.SeatId == reservationDto.SeatId);
            // if (seatReserved) return BadRequest("Seat is already reserved!");

            reservationModel.Seats.Add(seat);
            await _context.SaveChangesAsync();

            return Ok(reservationModel.ToReservationDto());
        }

        [HttpPost]
        [Route("{id:int}/cancel-seat")]
        public async Task<IActionResult> RemoveSeat([FromRoute] int id, [FromBody] ReservationSeatDto dto)
        {
            // Check if reservation exists
            var reservationModel = await _context.Reservations
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reservationModel == null) return NotFound("Reservation not found.");

            // Check if seat exists
            var seat = await _context.Seats.FindAsync(dto.SeatId);
            if (seat == null) return NotFound("Seat not found.");

            // Check if seat is already reserved
            // var seatReserved = await _context.Reservations
            //     .AnyAsync(r => r.ScreeningId == screeningId && r.SeatId == reservationDto.SeatId);
            // if (seatReserved) return BadRequest("Seat is already reserved!");

            reservationModel.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return Ok(reservationModel.ToReservationDto());
        }
    }
}