using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public SeatType Type { get; set; } = SeatType.Single;
    }
}