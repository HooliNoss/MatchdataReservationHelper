using System.Configuration;

namespace MatchdataReservationHelper
{
  public class RegisterTeamsConfig
    : ConfigurationSection
  {

    public static RegisterTeamsConfig GetConfig()
    {
      return (RegisterTeamsConfig)System.Configuration.ConfigurationManager.GetSection("RegisteredTeams") ?? new RegisterTeamsConfig();
    }

    [System.Configuration.ConfigurationProperty("Teams")]
    [ConfigurationCollection(typeof(Teams), 
      AddItemName = "Team")]
    public Teams Teams
    {
      get
      {
        object o = this["Teams"];
        return o as Teams;
      }
    }

  }
}
