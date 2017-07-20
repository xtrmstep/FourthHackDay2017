using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace datacleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            const int columnNumber = 7;
            var files = args;
            foreach (var file in files)
            {
                var directoryName = Path.GetDirectoryName(file);
                var fileName = Path.GetFileName(file).Replace(".csv", string.Empty); // removing extension for the name
                var outputFile = directoryName + fileName + "_adj.csv";
                var inputLines = File.ReadAllLines(file);
                var outputLines = new string[inputLines.Length];

                for (var index = 0; index < inputLines.Length; index++)
                {
                    var line = inputLines[index];
                    var parts = line.Split(',');
                    if (parts.Length > columnNumber)
                    {
                        // remove comma from name (it is the second comma in the line)
                        var commaPosition = line.IndexOf(',', 1);
                        commaPosition = line.IndexOf(',', commaPosition+1);
                        line = line.Remove(commaPosition, 1);
                    }
                    outputLines[index] = line;
                }

                File.WriteAllLines(outputFile, outputLines, Encoding.GetEncoding(1252)); // save in ansi
            }
        }
    }
}
