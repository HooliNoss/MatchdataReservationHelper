using System.Configuration;

namespace MatchdataReservationHelper
{
  public class Team : ConfigurationElement
  {
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
      get
      {
        return this["name"] as string;
      }
    }
    [ConfigurationProperty("swissVolleyName", IsRequired = true)]
    public string SwissVolleyName
    {
      get
      {
        return this["swissVolleyName"] as string;
      }
    }
    [ConfigurationProperty("sex", IsRequired = true)]
    public string Sex
    {
      get
      {
        return this["sex"] as string;
      }
    }
    [ConfigurationProperty("league", IsRequired = true)]
    public string League
    {
      get
      {
        return this["league"] as string;
      }
    }
    [ConfigurationProperty("group", IsRequired = true)]
    public string Group
    {
      get
      {
        return this["group"] as string;
      }
    }

  }
}
