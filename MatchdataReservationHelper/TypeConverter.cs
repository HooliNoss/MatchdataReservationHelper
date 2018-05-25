using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchdataReservationHelper
{
  public class TypeConverter
  {
    public static DateTime GetDateTimefromString(string aString)
    {
      DateTime retVal;
      if (DateTime.TryParse(aString, out retVal))
      {
        return retVal;
      }

      return DateTime.Parse(aString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
    }
  }
}
