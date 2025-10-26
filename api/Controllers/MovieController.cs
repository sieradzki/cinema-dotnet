using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Movie;
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
    public class MovieController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieRepository _movieRepo;
        public MovieController(ApplicationDbContext context, IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MovieQueryObject query)
        {
            var movies = await _movieRepo.GetAllAsync(query);
            var movieDto = movies.Select(m => m.ToMovieDto());
            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);

            if (movie == null) return NotFound();

            return Ok(movie.ToMovieDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequestDto movieDto)
        {
            var movieModel = movieDto.ToMovieFromCreateDto();

            await _movieRepo.CreateAsync(movieModel);

            return CreatedAtAction(nameof(GetById), new { id = movieModel.Id }, movieModel.ToMovieDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMovieRequestDto updateDto)
        {
            var movieModel = await _movieRepo.UpdateAsync(id, updateDto);
            if (movieModel == null) return NotFound();

            return Ok(movieModel.ToMovieDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var movieModel = await _movieRepo.DeleteAsync(id);
            if (movieModel == null) return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("{id:int}/assign-genre")]
        public async Task<IActionResult> AssignGenre([FromRoute] int id, [FromBody] MovieGenreDto dto)
        {
            // Check if movie exists
            var movieModel = await _context.Movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieModel == null) return NotFound("Movie not found.");

            // Check if genre exists
            var genre = await _context.Genres.FindAsync(dto.GenreId);
            if (genre == null) return NotFound("Genre not found.");

            // Check if movie already has that genre
            if (movieModel.Genres.Any(g => g.Id == dto.GenreId))
                return BadRequest("Movie already has that genre.");

            movieModel.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return Ok(movieModel.ToMovieDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}/remove-genre")]
        public async Task<IActionResult> RemoveGenre([FromRoute] int id, [FromBody] MovieGenreDto dto)
        {
            // Check if movie exists
            var movieModel = await _context.Movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieModel == null) return NotFound("Movie not found.");

            // Check if genre exists
            var genre = _context.Genres.FirstOrDefault(g => g.Id == dto.GenreId);
            if (genre == null) return NotFound("Genre is not assigned to this movie.");

            // Check if movie has that genre
            if (!movieModel.Genres.Any(g => g.Id == dto.GenreId))
                return BadRequest("Movie doesn't have that genre.");

            movieModel.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok(movieModel.ToMovieDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id:int}/upload-poster")]
        public async Task<IActionResult> UploadPoster(int id, IFormFile file)
        {
            // Check if movie exists
            var movieModel = await _context.Movies.FindAsync(id);
            if (movieModel == null) return NotFound("Movie not found.");

            // Check if file has been uploaded
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Check folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posters");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Get fileName and path
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            movieModel.PosterUrl = $"/images/posters/{fileName}";

            // Update movie // TODO should i do this with repo method? test later
            _context.Movies.Update(movieModel);
            await _context.SaveChangesAsync();

            return Ok(new { posterUrl = movieModel.PosterUrl });
        }
    }
}