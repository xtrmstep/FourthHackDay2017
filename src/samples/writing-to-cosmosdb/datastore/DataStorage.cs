using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace feeder.datastore
{
    public class DataStorage
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _collectionName;
        private readonly string _databaseName;
        private readonly string _endpointUrl;
        private readonly string _primaryKey;

        public DataStorage(string endpointUrl, string primaryKey, string databaseName, string collectionName)
        {
            _endpointUrl = endpointUrl;
            _primaryKey = primaryKey;
            _databaseName = databaseName;
            _collectionName = collectionName;
        }

        public async Task Put(string data)
        {
            using (var client = new DocumentClient(new Uri(_endpointUrl), _primaryKey))
            {
                var databaseName = _databaseName;
                var collectionName = _collectionName;
                try
                {
                    var document = JsonConvert.DeserializeObject(data);
                    var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
                    await client.CreateDocumentAsync(collectionUri, document);
                }
                catch (DocumentClientException e)
                {
                    _log.Error(e);
                    throw;
                }
            }
        }

        public static string Base64Encode(string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64Text)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}