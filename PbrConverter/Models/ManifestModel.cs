using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crews.Utility.PbrConverter.Models
{
    public class ManifestModel
    {
        [JsonProperty("format_version")]
        public int FormatVersion { get; set; }
        [JsonProperty("header")]
        public ManifestInfo Header { get; set; }
        [JsonProperty("modules")]
        public List<ManifestInfo> Modules { get; set; }
        [JsonProperty("capabilities")]
        public string[] Capabilities { get; set; }

        public class ManifestInfo
        {
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("uuid")]
            public string UUID { get; set; }
            public int[] Version { get; set; }
            [JsonProperty("min_engine_version")]
            public int[] MinEngineVersion { get; set; }
        }
    }
}
