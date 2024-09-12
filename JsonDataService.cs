using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace TotalCompensation2._0
{
    public class JsonDataService
    {
        private readonly string _filePath;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
        }

        public JsonData LoadJsonData()
        {
            string jsonData = File.ReadAllText(_filePath);
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss" // Match your JSON date format
            };
            return JsonConvert.DeserializeObject<JsonData>(jsonData, settings);
        }
    }
}
