using System;

namespace MatchdataReservationHelper.DTOs
{
  public class ReservationDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Hall { get; set; }
    }
}
