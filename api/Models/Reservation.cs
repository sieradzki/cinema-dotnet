using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string UserId { get; set; } // string by default in IdentityUser
        [Required]
        public int ScreeningId { get; set; }
        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        [ForeignKey(nameof(ScreeningId))]
        public Screening Screening { get; set; } = null!;
    }

    public enum ReservationStatus
    {
        Reserved = 0,
        Paid = 1,
        Cancelled = 2
    }
}