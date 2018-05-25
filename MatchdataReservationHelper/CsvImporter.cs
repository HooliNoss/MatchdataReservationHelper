using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using MatchdataReservationHelper.CsvTemplates;
using MatchdataReservationHelper.DTOs;

namespace MatchdataReservationHelper
{
    public class CsvImporter
    {
        public List<Match> ConvertCsvToList(string csvPath)
        {
            if (!File.Exists(csvPath))
            {
                throw new Exception("File does not exist");
            }

            var config = new CsvConfiguration()
            {
                HasHeaderRecord = true,
                WillThrowOnMissingField = false,
                Delimiter = ";",
            };

            config.RegisterClassMap<SvriTemplate>();

            using (var reader = new CsvReader(new StreamReader(csvPath, Encoding.GetEncoding("IBM852")), config))
            {
                return reader.GetRecords<Match>().ToList();
            }
        }
    }
}
