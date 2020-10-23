using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crews.Utility.PbrConverter.Models
{
    /// <summary>
    /// Represents application configuration file.
    /// </summary>
    public class ConfigurationModel
    {
        [JsonProperty("data")]
        public Data AppData { get; set; }

        public class Data
        {
            [JsonProperty("blocks")]
            public List<string> Blocks { get; set; }
        }
    }
}
