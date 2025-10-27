using api.Dtos.Reservation;
using api.Dtos.Seat;
using api.Models;

namespace api.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationDto ToReservationDto(this Reservation reservationModel)
        {
            return new ReservationDto
            {
                Id = reservationModel.Id,
                UserId = reservationModel.UserId,
                ScreeningId = reservationModel.ScreeningId,
                Status = reservationModel.Status,
                Seats = reservationModel.Seats
                    .Select(s => s.ToSeatDto())
                    .ToList()
            };
        }

        public static Reservation ToReservationFromCreateDto(this CreateReservationRequestDto reservationDto, int screeningId)
        {
            return new Reservation
            {
                UserId = reservationDto.UserId,
                ScreeningId = screeningId,
                Status = reservationDto.Status
            };
        }
    }
}
