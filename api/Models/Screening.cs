using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class Screening
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime TimeStart { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Precision(6, 2)]
        [Required]
        public decimal Price { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}