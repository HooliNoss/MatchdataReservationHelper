using System;
using MatchdataReservationHelper.DTOs;
using Spire.Doc;

namespace MatchdataReservationHelper.WordExport
{
  public static class ReservationGenerator
  {
    private static string OutputFolder = @"Output\";
    public static void GenerateGemeindeReservation(ReservationDto reservationDto)
    {
      string filename = OutputFolder + $"{reservationDto.Hall}_{reservationDto.StartTime.ToString("yyyy_MM_dd")}.docx";
      Document document = new Document();
      document.LoadFromFile(@"Vorlage_Buendtmaettli.docx");
      document.Replace("#DateOfUse", reservationDto.StartTime.ToString("dd.MM.yyyy"), false, true);
      document.Replace("#TimeOfUse", $"{reservationDto.StartTime.ToString("HH:mm")} - {reservationDto.EndTime.ToString("HH:mm")}", false, true);
      document.Replace("#CreationDate", DateTime.Now.ToString("dd.MM.yyyy"), false, true);
      document.SaveToFile(filename, FileFormat.Docx);
    }
  }
}
