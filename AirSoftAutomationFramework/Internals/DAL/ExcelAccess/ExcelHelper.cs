using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AirSoftAutomationFramework.Internals.DAL.ExcelAccess
{
    public static class ExcelHelper
    {
        public static string CreateCsvFile<T>(T actualObject, string fileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            using var writer = new StreamWriter(path);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, true);
            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
            csvWriter.WriteRecord(actualObject);
            csvWriter.NextRecord();

            var result = writer.ToString();

            return path;
        }

        public static string CreateCsvFile<T>(List<T> actualObject, string fileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, fileName);

            using var writer = new StreamWriter(path);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, true);
            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();

            foreach (var item in actualObject)
            {
                csvWriter.WriteRecord(item);
                csvWriter.NextRecord();
            }

            var result = writer.ToString();

            return path;
        }
    }
}
