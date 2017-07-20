using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace datacleaner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string OUTPUT_FILE = "output.csv";
            const string COLUMNS = "Locationid,RecipeName,PLU,Salesdate,Quantity,NetSalesPrice,CostPrice,Year,Month,Day,WeekDay,YearDay";
            const int COLUMN_NUMBER = 7;
            var files = args;

            File.AppendAllLines(OUTPUT_FILE, new[] {COLUMNS}, Encoding.GetEncoding(1252)); // save in ANSI

            foreach (var file in files)
            {
                Console.WriteLine("Processing: " + file);

                //var directoryName = Path.GetDirectoryName(file);
                //var fileName = Path.GetFileName(file).Replace(".csv", string.Empty); // removing extension for the name
                //var outputFile = directoryName + fileName + "_adj.csv";
                Console.WriteLine("Loading...");
                var inputLines = File.ReadAllLines(file);

                var linesCount = inputLines.Length;
                var outputLines = new string[linesCount];

                for (var index = 0; index < linesCount; index++)
                {
                    Console.Write("\rline {0}/{1} ({2}%)", index, linesCount, Math.Round(index/(float) linesCount*100, 2));

                    var line = inputLines[index];

                    #region remove all commas in the recipe name

                    var parts = line.Split(',');
                    while (parts.Length > COLUMN_NUMBER)
                    {
                        // remove comma from name (it is the second comma in the line)
                        var commaPosition = line.IndexOf(',', 1);
                        commaPosition = line.IndexOf(',', commaPosition + 1);
                        line = line.Remove(commaPosition, 1);
                        parts = line.Split(',');
                    }

                    #endregion

                    #region add year-month-day

                    parts = line.Split(',');
                    var datetime = DateTime.Parse(parts[3]);
                    line += string.Format("{0},{1},{2},{3},{4}", datetime.Year, datetime.Month, datetime.Day, (int)datetime.DayOfWeek, datetime.DayOfYear);

                    #endregion

                    outputLines[index] = line;
                }
                Console.WriteLine("Processed");

                //File.WriteAllLines(file, outputLines, Encoding.GetEncoding(1252)); // save in ANSI


                File.AppendAllLines(OUTPUT_FILE, outputLines, Encoding.GetEncoding(1252)); // save in ANSI
                Console.WriteLine("Saved");
            }
        }
    }
}