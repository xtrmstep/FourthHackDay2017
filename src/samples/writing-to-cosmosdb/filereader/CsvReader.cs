using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace feeder.filereader
{
    /// <summary>
    ///     Very simple CSV file reader
    /// </summary>
    /// <remarks>
    ///     The helper reads CSV record by record without loading whole data to memory
    /// </remarks>
    public class CsvReader : ICsvReader, IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _filePath;
        private readonly bool _hasColumnNames;
        private readonly char _separator;
        private string[] _columnNames = {};
        private StreamReader _fileStream;

        /// <summary>
        ///     Initialize the instance if the CSV file reader
        /// </summary>
        /// <param name="filePath">Absolute path to CSV file</param>
        /// <param name="separator">Columns separator character. Comma (,) by default</param>
        /// <param name="hasColumnNames">Determine if the first line in the file defines column headers</param>
        public CsvReader(string filePath, char separator = ',', bool hasColumnNames = true)
        {
            _filePath = filePath;
            _separator = separator;
            _hasColumnNames = hasColumnNames;
        }

        public bool Open()
        {
            try
            {
                _fileStream = File.OpenText(_filePath);
                if (_hasColumnNames)
                {
                    var firstLine = _fileStream.ReadLine();
                    _columnNames = firstLine.Split(_separator);
                }
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
        }

        public void Close()
        {
            _fileStream.Dispose();
            _fileStream = null;
        }

        public IEnumerable<string> ReadJsonToEnd()
        {
            if (_fileStream == null)
                throw new InvalidOperationException("Cannot read from the closed stream.");

            var line = _fileStream.ReadLine();
            while (line != null)
            {
                var json = CreateJson(line);
                yield return json;
                line = _fileStream.ReadLine();
            }
        }

        public void Dispose()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
        }

        private string CreateJson(string line)
        {
            var items = line.Split(_separator);

            var sb = new StringBuilder("{");
            for (var i = 0; i < items.Length; i++)
            {
                var key = "Column" + i;
                if (i < _columnNames.Length)
                    key = _columnNames[i];

                var jsonValue = items[i];
                if (!jsonValue.IsNumeric())
                    jsonValue = string.Format(@"""{0}""", jsonValue);
                sb.AppendFormat(@"""{0}"": {1},", key, jsonValue);
            }
            sb.Remove(sb.Length - 1, 1); // remove last comma (,)
            sb.Append("}");
            return sb.ToString();
        }
    }
}