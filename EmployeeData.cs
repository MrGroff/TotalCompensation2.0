using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCompensation2._0
{
    public class EmployeeData
    {
        public string Employee { get; set; }
        [JsonProperty("timePunch")]
        public List<TimePunch> TimePunches { get; set; }
    }
}
