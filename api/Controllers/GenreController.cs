using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Genre;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenreRepository _genreRepo;
        public GenreController(ApplicationDbContext context, IGenreRepository genreRepo)
        {
            _genreRepo = genreRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreRepo.GetAllAsync();
            var genreDto = genres.Select(g => g.ToGenreDto());

            return Ok(genres);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var genre = await _genreRepo.GetByIdAsync(id);

            if (genre == null) return NotFound();

            return Ok(genre.ToGenreDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGenreRequestDto genreDto)
        {
            var genreModel = genreDto.ToGenreFromCreateDto();

            await _genreRepo.CreateAsync(genreModel);

            return CreatedAtAction(nameof(GetById), new { id = genreModel.Id }, genreModel.ToGenreDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGenreRequestDto updateDto)
        {
            var genreModel = await _genreRepo.UpdateAsync(id, updateDto);
            if (genreModel == null) return NotFound();

            return Ok(genreModel.ToGenreDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var genreModel = await _genreRepo.DeleteAsync(id);
            if (genreModel == null) return NotFound();

            return NoContent();
        }
    }
}