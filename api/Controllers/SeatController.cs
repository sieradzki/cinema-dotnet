using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Room;
using api.Dtos.Seat;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISeatRepository _seatRepo;
        private readonly IRoomRepository _roomRepo;
        public SeatController(
            ApplicationDbContext context,
            ISeatRepository seatRepo,
            IRoomRepository roomRepo
        )
        {
            _seatRepo = seatRepo;
            _roomRepo = roomRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _seatRepo.GetAllAsync();
            var seatDto = seats.Select(s => s.ToSeatDto());

            return Ok(seats);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var seat = await _seatRepo.GetByIdAsync(id);

            if (seat == null) return NotFound();

            return Ok(seat.ToSeatDto());
        }

        [HttpPost("{roomId:int}")]
        public async Task<IActionResult> Create([FromRoute] int roomId, [FromBody] CreateSeatRequestDto seatDto)
        {
            if (!await _roomRepo.RoomExists(roomId))
                return BadRequest("Room does not exist!");

            var seatModel = seatDto.ToSeatFromCreateDto(roomId);

            await _seatRepo.CreateAsync(seatModel);

            return CreatedAtAction(nameof(GetById), new { id = seatModel.Id }, seatModel.ToSeatDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSeatRequestDto updateDto)
        {
            var seatModel = await _seatRepo.UpdateAsync(id, updateDto.ToSeatFromUpdateDto());
            if (seatModel == null) return NotFound();

            return Ok(seatModel.ToSeatDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var seatModel = await _seatRepo.DeleteAsync(id);
            if (seatModel == null) return NotFound();

            return NoContent();
        }
    }
}