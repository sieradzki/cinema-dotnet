using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Screening;
using api.Models;

namespace api.Mappers
{
    public static class ScreeningMapper
    {
        public static ScreeningDto ToScreeningDto(this Screening screeningModel)
        {
            return new ScreeningDto
            {
                Id = screeningModel.Id,
                TimeStart = screeningModel.TimeStart,
                RoomId = screeningModel.RoomId,
                MovieId = screeningModel.MovieId,
                Price = screeningModel.Price
            };
        }

        public static ScreeningWithReservationsDto ToScreeningWithReservationsDto(this Screening screeningModel)
        {
            return new ScreeningWithReservationsDto
            {
                Id = screeningModel.Id,
                TimeStart = screeningModel.TimeStart,
                RoomId = screeningModel.RoomId,
                MovieId = screeningModel.MovieId,
                Price = screeningModel.Price,
                Reservations = screeningModel.Reservations.Select(r => r.ToReservationDto()).ToList()
            };
        }

        public static Screening ToScreeningFromCreateDto(this CreateScreeningRequestDto screeningDto, int movieId)
        {
            return new Screening
            {
                TimeStart = screeningDto.TimeStart,
                RoomId = screeningDto.RoomId,
                MovieId = movieId,
                Price = screeningDto.Price
            };
        }

        public static Screening ToScreeningFromUpdateDto(this UpdateScreeningRequestDto screeningDto)
        {
            return new Screening
            {
                TimeStart = screeningDto.TimeStart,
                RoomId = screeningDto.RoomId,
                MovieId = screeningDto.MovieId,
                Price = screeningDto.Price
            };
        }
    }
}