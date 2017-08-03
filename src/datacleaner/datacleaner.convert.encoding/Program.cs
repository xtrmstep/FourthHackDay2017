using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datacleaner.convert.encoding
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = args;

            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd317756(v=vs.85).aspx
            var ANSI = Encoding.GetEncoding(1252);
            var UTF8 = Encoding.GetEncoding(65001);

            foreach (var file in files)
            {
                var directoryName = Path.GetDirectoryName(file);
                var fileName = Path.GetFileName(file).Replace(".csv", string.Empty); // removing extension for the name
                var outputFile = directoryName + fileName + "_utf8.csv";

                Console.WriteLine("Processing: " + file);
                var lines = File.ReadAllLines(file, ANSI);
                File.WriteAllLines(outputFile, lines, UTF8);
                Console.WriteLine("Processed");
            }
            Console.WriteLine("All files are processed");
            Console.ReadKey();
        }
    }
}
