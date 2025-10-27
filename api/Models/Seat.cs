using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Number { get; set; }
        public SeatType Type { get; set; } = SeatType.Single;
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;
    }

    public enum SeatType
    {
        Single = 0,
        Couch = 1,
        Accessible = 2
    }
}