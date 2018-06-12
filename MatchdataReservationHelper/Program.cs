using MatchdataReservationHelper.DTOs;
using MatchdataReservationHelper.WordExport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatchdataReservationHelper
{
  class Program
  {
    private static string OutputFolder = @"Output\";

    [STAThread]
    static void Main(string[] args)
    {
      try
      {
        ShowStartupInfos();

        Console.WriteLine("Drücke Enter um das CSV-File mit dem Spielplan zu definieren");
        Console.ReadLine();
        var csvImporter = new CsvImporter();
        string file = OpenCsvFile();
        if (string.IsNullOrEmpty(file))
        {
          Console.WriteLine("Kein gültiger Spielplan gewählt. Programm wird beendet");
          Console.ReadLine();
          return;
        }

        double preparingTime = Convert.ToDouble(ConfigurationManager.AppSettings["PreperingTimeInHours"]) * -1;
        double playTime = Convert.ToDouble(ConfigurationManager.AppSettings["AverageMatchTimeInHours"]);

        Console.WriteLine($"CSV File: {file}");
        Console.WriteLine("");

        CreateOutputFolder();

        GenerateSpielplanVbcMalters(csvImporter.ConvertCsvToList(file));
        GenerateOverview(csvImporter.ConvertCsvToList(file), preparingTime, playTime);
        GenerateReservationFiles(csvImporter.ConvertCsvToList(file), preparingTime, playTime);

        Console.ReadLine();

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Console.WriteLine("edit config file to change filename and indexes");

        Console.ReadLine();
      }
    }

    private static void ShowStartupInfos()
    {
      Console.WriteLine("Das Programm liest den Gesamtspielplan welcher von der myVolley site generiert wurde.");
      Console.WriteLine("Im generierten File Gemeindereservationszeiten.txt werden die Reservierungszeiten angezeigt, welche man der Gemeinde frühzeitig mitteilen kann.");
      Console.WriteLine("Im generierten File Spielplan_VBCMalters.csv ist der Gesamtspielplan zu finden.");
      Console.WriteLine("Es werden alle Reservationen für die Gemeinde erstellt (Achtung alles im Bündtmättliformat).");
      Console.WriteLine("Sämtliche Files sind im Ordner Output zu finden");
      Console.WriteLine("");
      Console.WriteLine("Die Teams können im .Config File angepasst werden");
      Console.WriteLine("");
      Console.WriteLine("");
    }

    private static void CreateOutputFolder()
    {
      if (Directory.Exists(OutputFolder))
      {
        DirectoryInfo di = new DirectoryInfo(OutputFolder);
        foreach (FileInfo file in di.GetFiles())
        {
          file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
          dir.Delete(true);
        }
      }
      else
      {
        Directory.CreateDirectory(OutputFolder);
      }
    }

    private static string OpenCsvFile()
    {
      OpenFileDialog dialog = new OpenFileDialog
      {
        Filter = "csv files (*.csv)|*.csv",
        RestoreDirectory = true
      };
      var result = dialog.ShowDialog();

      if (result == DialogResult.OK)
      {
        return dialog.FileName;
      }

      return null;
    }

    private static void GenerateOverview(List<Match> matchList, double preparingTime, double playTime)
    {
      matchList.RemoveAll(match => !(match.DateTime.DayOfWeek == DayOfWeek.Saturday || match.DateTime.DayOfWeek == DayOfWeek.Sunday));
      matchList.RemoveAll(match => !match.HomeTeam.Contains("Malters"));

      var orderedMatchlist = matchList.OrderBy(match => match.DateTime).ToList();

      List<DateTime> allMatchDays = orderedMatchlist.Select(match => match.DateTime)
              .Select(date => new DateTime(date.Year, date.Month, date.Day))
              .Distinct()
              .ToList();


      using (var writetext = new StreamWriter(OutputFolder + "Gemeindereservationszeiten.txt"))
      {
        foreach (var matchDay in allMatchDays)
        {
          Match firstMatchOfDay = orderedMatchlist.First(match => match.DateTime.ToString("dd.MM.yyyy") == matchDay.ToString("dd.MM.yyyy"));
          Match lastMatchOfDay = orderedMatchlist.Last(match => match.DateTime.ToString("dd.MM.yyyy") == matchDay.ToString("dd.MM.yyyy"));


          string output = $"{matchDay:dd.MM.yyyy} " +
                          $"{firstMatchOfDay.DateTime.AddHours(preparingTime):HH:mm} - " +
                          $"{lastMatchOfDay.DateTime.AddHours(playTime):HH:mm}   {firstMatchOfDay.Hall}";

          Console.WriteLine(output);
          writetext.WriteLine(output);
        }
      }
    }

    private static void GenerateReservationFiles(List<Match> matchList, double preparingTime, double playTime)
    {
      matchList.RemoveAll(match => !(match.DateTime.DayOfWeek == DayOfWeek.Saturday || match.DateTime.DayOfWeek == DayOfWeek.Sunday));
      matchList.RemoveAll(match => !match.HomeTeam.Contains("Malters"));

      List<Match> orderedMatchlist = matchList.OrderBy(match => match.DateTime).ToList();

      List<DateTime> allMatchDays = orderedMatchlist.Select(match => match.DateTime)
              .Select(date => new DateTime(date.Year, date.Month, date.Day))
              .Distinct()
              .ToList();

      foreach (var matchDay in allMatchDays)
      {
        Match firstMatchOfDay = orderedMatchlist.First(match => match.DateTime.ToString("dd.MM.yyyy") == matchDay.ToString("dd.MM.yyyy"));
        Match lastMatchOfDay = orderedMatchlist.Last(match => match.DateTime.ToString("dd.MM.yyyy") == matchDay.ToString("dd.MM.yyyy"));


        var dto = new ReservationDto()
        {
          StartTime = firstMatchOfDay.DateTime.AddHours(preparingTime),
          EndTime = lastMatchOfDay.DateTime.AddHours(playTime),
          Hall = firstMatchOfDay.Hall
        };
        ReservationGenerator.GenerateGemeindeReservation(dto);
      }
    }

    private static void GenerateSpielplanVbcMalters(List<Match> matchList)
    {
      var orderedMatchlist = matchList.OrderBy(match => match.DateTime).ToList();


      foreach (var match in orderedMatchlist)
      {
        match.HomeTeam = ReplaceTeamname(match.HomeTeam, match.League, match.Group);
        match.AwayTeam = ReplaceTeamname(match.AwayTeam, match.League, match.Group);
      }

      using (var sw = new StreamWriter(new FileStream(OutputFolder + "Spielplan_VBCMalters.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite), Encoding.UTF8))
      {
        string header = "Datum;Wochentag;Anspielzeit;Liga;Heimteam;Gastteam;Halle";
        Console.WriteLine(header);
        sw.WriteLine(header);

        foreach (var match in orderedMatchlist)
        {
          string output = $"{match.DateTime:dd.MM.yyyy HH:mm};" +
                          $"{DateTimeFormatInfo.CurrentInfo.GetDayName(match.DateTime.DayOfWeek)};" +
                          $"{match.DateTime:HH:mm};" +
                          $"{match.League};" +
                          $"{match.HomeTeam};" +
                          $"{match.AwayTeam};" +
                          $"{match.Hall}";

          Console.WriteLine(output);
          sw.WriteLine(output);
        }
      }
    }

    private static string ReplaceTeamname(string teamName, string league, string group)
    {

      var teamsConfig = RegisterTeamsConfig.GetConfig();

      foreach (Team team in teamsConfig.Teams)
      {
        if (teamName == team.SwissVolleyName &&
            league == team.League &&
            group == team.Group)
        {
          return team.Name;
        }
      }
      return teamName;
    }
  }
}
