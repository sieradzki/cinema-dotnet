using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Screening;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreeningController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IScreeningRepository _screeningRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IRoomRepository _roomRepo;
        public ScreeningController(
            ApplicationDbContext context,
            IScreeningRepository screeningRepo,
            IMovieRepository movieRepo,
            IRoomRepository roomRepo
        )
        {
            _context = context;
            _screeningRepo = screeningRepo;
            _movieRepo = movieRepo;
            _roomRepo = roomRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ScreeningQueryObject queryObject)
        {
            var screenings = await _screeningRepo.GetAllWithReservationsAsync(queryObject);
            var screeningDto = screenings.Select(s => s.ToScreeningWithReservationsDto());

            return Ok(screenings);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdWithReservations([FromRoute] int id)
        {
            var screening = await _screeningRepo.GetByIdWithReservationsAsync(id);

            if (screening == null) return NotFound();

            return Ok(screening.ToScreeningWithReservationsDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{movieId:int}")]
        public async Task<IActionResult> Create([FromRoute] int movieId, [FromBody] CreateScreeningRequestDto screeningDto)
        {
            // Check if movie exists
            if (!await _movieRepo.MovieExists(movieId))
                return BadRequest("Movie does not exist!");

            // Check if room exists
            if (!await _roomRepo.RoomExists(screeningDto.RoomId))
                return BadRequest("Room does not exist.");

            var screeningModel = screeningDto.ToScreeningFromCreateDto(movieId);

            // Check overlapping and time constraints
            var movie = await _movieRepo.GetByIdAsync(movieId);
            var screeningStart = screeningDto.TimeStart;
            var screeningEnd = screeningStart.AddMinutes(movie!.Duration);

            var localTimeStart = screeningStart.TimeOfDay;
            var localTimeEnd = screeningEnd.TimeOfDay;

            // Only between 8 and 23
            if (localTimeStart < TimeSpan.FromHours(8) || localTimeEnd > TimeSpan.FromHours(23))
                return BadRequest("Screenings must be scheduled between 8:00 and 23:00.");

            // Check if the screenings overlap including 15 minute ad breaks 
            var bufferedStart = screeningStart.AddMinutes(-15);
            var bufferedEnd = screeningEnd.AddMinutes(15);
            var screeningDate = screeningStart.Date;

            var overlaps = await (
                from s in _context.Screenings
                join m in _context.Movies on s.MovieId equals m.Id
                where s.RoomId == screeningDto.RoomId
                where s.TimeStart.Date == screeningDate
                where s.TimeStart.AddMinutes(m.Duration) > bufferedStart
                where s.TimeStart < bufferedEnd
                select s
            ).AnyAsync();

            if (overlaps) return Conflict("Another screening is already scheduled in this room.");

            await _screeningRepo.CreateAsync(screeningModel);

            return CreatedAtAction(nameof(GetByIdWithReservations), new { id = screeningModel.Id }, screeningModel.ToScreeningDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateScreeningRequestDto updateDto)
        {
            // Check if movie exists
            if (!await _movieRepo.MovieExists(updateDto.MovieId))
                return BadRequest("Movie does not exist!");

            // Check if room exists
            if (!await _roomRepo.RoomExists(updateDto.RoomId))
                return BadRequest("Room does not exist.");

            // Check overlapping and time constraints
            var movie = await _movieRepo.GetByIdAsync(updateDto.MovieId);
            var screeningStart = updateDto.TimeStart;
            var screeningEnd = screeningStart.AddMinutes(movie!.Duration);

            var localTimeStart = screeningStart.TimeOfDay;
            var localTimeEnd = screeningEnd.TimeOfDay;

            // Only between 8 and 23
            if (localTimeStart < TimeSpan.FromHours(8) || localTimeEnd > TimeSpan.FromHours(23))
                return BadRequest("Screenings must be scheduled between 8:00 and 23:00.");

            var screeningDate = screeningStart.Date;
            // Check if the screenings overlap including 15 minute ad breaks 
            var bufferedStart = screeningStart.AddMinutes(30);
            var bufferedEnd = screeningEnd.AddMinutes(30);
            var overlaps = await (
                from s in _context.Screenings
                join m in _context.Movies on s.MovieId equals m.Id
                where s.RoomId == updateDto.RoomId
                where s.TimeStart.Date == screeningDate
                where s.TimeStart.AddMinutes(m.Duration) > bufferedStart
                where s.TimeStart < bufferedEnd
                select s
            ).AnyAsync();

            if (overlaps) return Conflict("Another screening is already scheduled in this room.");

            var screeningModel = await _screeningRepo.UpdateAsync(id, updateDto);
            if (screeningModel == null) return NotFound();

            return Ok(screeningModel.ToScreeningDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var screeningModel = await _screeningRepo.DeleteAsync(id);
            if (screeningModel == null) return NotFound();

            return NoContent();
        }
    }
}