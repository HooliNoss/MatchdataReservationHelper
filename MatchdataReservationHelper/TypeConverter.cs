using System;
using System.Globalization;

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
