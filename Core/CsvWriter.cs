using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourserWork.Core
{
    public class CsvWriter
    {
        public static void WriteToCsv(List<(int Key, double Value)> results, string folderPath)
        {
            try
            {
                const string fileName = "FixedFileName.csv";
                string filePath = Path.Combine(folderPath, fileName);

                var csvString = "Simulation Time,Revenue\n";
                foreach (var result in results)
                {
                    csvString += $"{result.Key},{result.Value}\n";
                }

                File.WriteAllText(filePath, csvString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing CSV: {ex.Message}");
            }
        }
    }
}
