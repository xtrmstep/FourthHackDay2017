using System.Collections.Generic;

namespace feeder.filereader
{
    public interface ICsvReader
    {
        bool Open();
        IEnumerable<string> ReadJsonToEnd();

        void Close();
    }
}