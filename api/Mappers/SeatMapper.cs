using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Seat;
using api.Models;

namespace api.Mappers
{
    public static class SeatMapper
    {
        public static SeatDto ToSeatDto(this Seat seatModel)
        {
            return new SeatDto
            {
                Id = seatModel.Id,
                RoomId = seatModel.RoomId,
                Row = seatModel.Row,
                Number = seatModel.Number,
                Type = seatModel.Type
            };
        }

        public static Seat ToSeatFromCreateDto(this CreateSeatRequestDto seatDto, int roomId)
        {
            return new Seat
            {
                RoomId = roomId,
                Row = seatDto.Row,
                Number = seatDto.Number,
                Type = seatDto.Type
            };
        }

         public static Seat ToSeatFromUpdateDto(this UpdateSeatRequestDto seatDto)
        {
            return new Seat
            {
                Row = seatDto.Row,
                Number = seatDto.Number,
                Type = seatDto.Type
            };
        }
    }
}