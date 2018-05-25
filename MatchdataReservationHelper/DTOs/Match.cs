using System;

namespace MatchdataReservationHelper.DTOs
{
    public class Match
    {
        public DateTime DateTime { get; set; }
        public string Hall { get; set; }
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string League { get; set; }

        public string Group { get; set; }
    }
}
