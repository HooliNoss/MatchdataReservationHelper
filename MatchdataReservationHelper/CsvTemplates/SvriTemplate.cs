using System;
using System.Configuration;
using CsvHelper.Configuration;
using MatchdataReservationHelper.DTOs;

namespace MatchdataReservationHelper.CsvTemplates
{
    public class SvriTemplate : CsvClassMap<Match>
    {
        public SvriTemplate()
        {
            string dateTimeIndex = ConfigurationManager.AppSettings["DateTimeIndex"];
            string hallIndex = ConfigurationManager.AppSettings["HallIndex"];
            string homeTeamIndex = ConfigurationManager.AppSettings["HomeTeamIndex"];
            string awayTeamIndex = ConfigurationManager.AppSettings["AwayTeamIndex"];
            string leagueIndex = ConfigurationManager.AppSettings["LeagueIndex"];
            string groupIndex = ConfigurationManager.AppSettings["GroupIndex"];


            Map(m => m.Hall).Index(Convert.ToInt32(hallIndex));
            Map(m => m.DateTime).ConvertUsing(row => TypeConverter.GetDateTimefromString($"{row[Convert.ToInt32(dateTimeIndex)]}"));
            Map(m => m.HomeTeam).Index(Convert.ToInt32(homeTeamIndex));
            Map(m => m.AwayTeam).Index(Convert.ToInt32(awayTeamIndex));
            Map(m => m.League).Index(Convert.ToInt32(leagueIndex));
            Map(m => m.Group).Index(Convert.ToInt32(groupIndex));
        }
    }
}
