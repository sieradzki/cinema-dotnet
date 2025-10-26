using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Seat;

namespace api.Dtos.Room
{
    public class RoomDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class RoomWithSeatsDto : RoomDto
    {
        public List<SeatDto>? Seats { get; set; }
    }
}