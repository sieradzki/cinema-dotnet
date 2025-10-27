using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public required string Title { get; set; }
        [MaxLength(2048)]
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        [Range(1, 1000)]
        public int Duration { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
    }
}