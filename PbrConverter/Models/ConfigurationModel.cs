using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crews.Utility.PbrConverter.Models
{
    /// <summary>
    /// Represents application configuration file.
    /// </summary>
    public class ConfigurationModel
    {
        /// <summary>
        /// Primary application data member.
        /// </summary>
        [JsonProperty("data")]
        public Data AppData { get; set; }

        /// <summary>
        /// Represents an application data object.
        /// </summary>
        public class Data
        {
            /// <summary>
            /// Compatible texture set version.
            /// </summary>
            [JsonProperty("texture-set-version")]
            public string TextureSetVersion { get; set; }

            /// <summary>
            /// List of supported texture names.
            /// </summary>
            [JsonProperty("textures")]
            public List<string> Textures { get; set; }
        }
    }
}
