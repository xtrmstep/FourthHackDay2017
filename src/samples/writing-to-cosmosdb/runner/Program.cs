﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using feeder.datastore;
using feeder.filereader;
using feeder.runner.Properties;
using log4net;
using log4net.Config;

namespace feeder.runner
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            _log.Debug("Session started.");

            string[] files;
            #region find files
            if (args.Length == 0)
            {
                var dataFolder = Settings.Default.DataFolder;
                var runningFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dataFilesFolder = runningFolder + dataFolder;
                var csvFiles = Directory.EnumerateFiles(dataFilesFolder, "*.csv");
                files = csvFiles as string[] ?? csvFiles.ToArray();

                _log.DebugFormat("Folder: {0}", dataFilesFolder);
                _log.Info("CSV files are found.");
            }
            else
            {
                files = new[] { args[0] };
            } 
            #endregion

            _log.DebugFormat("Found {0} CSV files", files.Length);
            _log.Info("Processing...");

            var store = new DataStorage(Settings.Default.EndpointUrl, Settings.Default.PrimaryKey, Settings.Default.DatabaseName, Settings.Default.CollectionName);
            foreach (var file in files)
            {
                _log.DebugFormat("Reading file {0}", file);

                using (var reader = new CsvReader(file))
                {
                    reader.Open();
                    foreach (var json in reader.ReadJsonToEnd())
                    {
                        store.Put(json).Wait();
                    }
                }
            }
            
            _log.Info("All files are processed.");
            _log.Debug("Session finished.");
        }
    }
}
