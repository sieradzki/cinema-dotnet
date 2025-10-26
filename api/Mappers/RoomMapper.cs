using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Genre;
using api.Dtos.Room;
using api.Models;

namespace api.Mappers
{
    public static class RoomMapper
    {
        public static RoomDto ToRoomDto(this Room roomModel)
        {
            return new RoomDto
            {
                Id = roomModel.Id,
                Name = roomModel.Name
            };
        }

        public static RoomWithSeatsDto ToRoomWithSeatsDto(this Room roomModel)
        {
            return new RoomWithSeatsDto
            {
                Id = roomModel.Id,
                Name = roomModel.Name,
                Seats = roomModel.Seats.Select(s => s.ToSeatDto()).ToList()
            };
        }

        public static Room ToRoomFromCreateDto(this CreateRoomRequestDto roomDto)
        {
            return new Room
            {
                Name = roomDto.Name
            };
        }
    }
}