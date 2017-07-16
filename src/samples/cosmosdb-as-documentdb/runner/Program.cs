using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using feeder.filereader;
using feeder.runner.Properties;
using log4net;

namespace feeder.runner
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _log.Debug("Session started.");

            string[] files;
            if (args.Length == 0)
            {
                var dataFolder = Settings.Default.DataFolder;
                var runningFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dataFilesFolder = Path.Combine(runningFolder, dataFolder);
                var csvFiles = Directory.EnumerateFiles(dataFilesFolder, "*.csv");
                files = csvFiles as string[] ?? csvFiles.ToArray();

                _log.DebugFormat("Folder: {0}", dataFilesFolder);
                _log.Info("CSV files are found.");
            }
            else
            {
                files = new[] {args[0]};
            }

            _log.DebugFormat("Found {0} CSV files", files.Length);
            _log.Info("Processing...");

            foreach (var file in files)
            {
                _log.DebugFormat("Reading file {0}", file);

                using (var reader = new CsvReader(file))
                {
                    reader.Open();
                    foreach (var json in reader.ReadJsonToEnd())
                    {
                        // send json to cosmos
                    }
                }
            }

            _log.Info("All files are processed.");
            _log.Debug("Session finished.");
        }
    }
}
