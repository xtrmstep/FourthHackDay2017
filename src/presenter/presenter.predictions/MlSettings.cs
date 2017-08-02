using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace presenter.predictions
{
    public class MlSettings
    {
        // v1
        //public const string Key = "g9V57Y70SwkdrfARqQn4+IpoH9AW8wfYUAWc3c9e0D9Bs+L6EQSmNtaOPpdRe/rtECNwswADPQWIOnA8IBocmA==";
        //public const string Endpoint = "https://europewest.services.azureml.net/subscriptions/d2c4886722054839b9443d7514832781/services/a48218bd0d554423ac616028acf24aa4/execute?api-version=2.0&format=swagger";
        //public const string ScoredValueFieldName = "Scored Labels";

        // v2
        public const string Key = "2P0BTugn90z+I+eHvYf3i5P3BRcbLi++IOa2rh7nVHWqtq0uVhzwXeO+EnbG2Az18/UVoIMljQsrDjki69L1FA==";
        public const string Endpoint = "https://europewest.services.azureml.net/subscriptions/d2c4886722054839b9443d7514832781/services/fbab836adc934765a47f832eb2ef9793/execute?api-version=2.0&format=swagger";
        public const string ScoredValueFieldName = "Scored Label Mean";
    }
}
