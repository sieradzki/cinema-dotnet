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
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? PosterUrl { get; set; }
        public int Duration { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Screening> Screenings { get; set; } = new List<Screening>(); 
    }
}