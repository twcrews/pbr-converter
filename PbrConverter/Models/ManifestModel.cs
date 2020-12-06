using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crews.Utility.PbrConverter.Models
{
    /// <summary>
    /// Represents the manifest.json file in a resource pack.
    /// </summary>
    public class ManifestModel
    {
        /// <summary>
        /// The resource pack format version number.
        /// </summary>
        [JsonProperty("format_version")]
        public int FormatVersion { get; set; }

        /// <summary>
        /// The manifest info header.
        /// </summary>
        [JsonProperty("header")]
        public ManifestInfo Header { get; set; }

        /// <summary>
        /// The modules contained in the manifest.
        /// </summary>
        [JsonProperty("modules")]
        public List<ManifestInfo> Modules { get; set; }

        /// <summary>
        /// The capabilities of the resource pack.
        /// </summary>
        [JsonProperty("capabilities")]
        public string[] Capabilities { get; set; }

        /// <summary>
        /// Represents a manifest information object.
        /// </summary>
        public class ManifestInfo
        {
            /// <summary>
            /// Description of the information object.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// Name of the information object.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Type of the information object.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// Unique identifier of the information object.
            /// </summary>
            [JsonProperty("uuid")]
            public string UUID { get; set; }

            /// <summary>
            /// Version of the information object.
            /// </summary>
            [JsonProperty("version")]
            public int[] Version { get; set; }

            /// <summary>
            /// Minimum engine version supported by the information object.
            /// </summary>
            [JsonProperty("min_engine_version")]
            public int[] MinEngineVersion { get; set; }
        }
    }
}
