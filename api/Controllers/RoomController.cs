using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Room;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoomRepository _roomRepo;

        public RoomController(ApplicationDbContext context, IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomRepo.GetAllWithSeatsAsync();
            var roomDto = rooms.Select(r => r.ToRoomDto());

            return Ok(rooms);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithSeats([FromRoute] int id)
        {
            var room = await _roomRepo.GetByIdWithSeatsAsync(id);

            if (room == null) return NotFound();

            return Ok(room.ToRoomWithSeatsDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto roomDto)
        {
            // Check if a room with the same already exists
            var nameExists = await _context.Rooms.AnyAsync(r => r.Name == roomDto.Name);
            if (nameExists)
            {
                ModelState.AddModelError("Name", "A room with the same name already exists.");
                return BadRequest(ModelState);
            }

            var roomModel = roomDto.ToRoomFromCreateDto();

            await _roomRepo.CreateAsync(roomModel);

            return CreatedAtAction(nameof(GetByIdWithSeats), new { id = roomModel.Id }, roomModel.ToRoomDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoomRequestDto updateDto)
        {
            // Check if a room with the same already exists
            var nameExists = await _context.Rooms.AnyAsync(r => r.Id != id && r.Name == updateDto.Name);
            if (nameExists)
            {
                ModelState.AddModelError("Name", "A room with the same name already exists.");
                return BadRequest(ModelState);
            }

            var roomModel = await _roomRepo.UpdateAsync(id, updateDto);
            if (roomModel == null) return NotFound();

            return Ok(roomModel.ToRoomDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var roomModel = await _roomRepo.DeleteAsync(id);
            if (roomModel == null) return NotFound();

            return NoContent();
        }
    }
}