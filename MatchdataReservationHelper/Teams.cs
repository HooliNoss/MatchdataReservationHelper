using System.Configuration;

namespace MatchdataReservationHelper
{
  public class Teams
    : ConfigurationElementCollection
  {
    public Team this[int index]
    {
      get
      {
        return base.BaseGet(index) as Team;
      }
      set
      {
        if (base.BaseGet(index) != null)
        {
          base.BaseRemoveAt(index);
        }
        this.BaseAdd(index, value);
      }
    }

    public new Team this[string responseString]
    {
      get { return (Team)BaseGet(responseString); }
      set
      {
        if (BaseGet(responseString) != null)
        {
          BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
        }
        BaseAdd(value);
      }
    }

    protected override System.Configuration.ConfigurationElement CreateNewElement()
    {
      return new Team();
    }

    protected override object GetElementKey(System.Configuration.ConfigurationElement element)
    {
      return ((Team)element).Name;
    }
  }
}
