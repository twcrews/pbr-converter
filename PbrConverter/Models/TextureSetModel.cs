﻿using Newtonsoft.Json;

namespace Crews.Utility.PbrConverter.Models
{
    /// <summary>
    /// Represents a texture set model JSON object.
    /// </summary>
    public class TextureSetModel
    {
        /// <summary>
        /// The format version of the texture.
        /// </summary>
        [JsonProperty("format_version")]
        public string FormatVersion { get; set; }

        /// <summary>
        /// The texture set information object of the texture.
        /// </summary>
        [JsonProperty("minecraft:texture_set")]
        public TextureSetInfo MinecraftTextureSet { get; set; }

        /// <summary>
        /// Represents a texture set information object.
        /// </summary>
        public class TextureSetInfo
        {
            /// <summary>
            /// The path or values of the color texture.
            /// </summary>
            [JsonProperty("color")]
            public object Color { get; set; }

            /// <summary>
            /// The path or values of the MER texture.s
            /// </summary>
            [JsonProperty("metalness_emissive_roughness")]
            public object MER { get; set; }

            /// <summary>
            /// The path of the Normal texture.
            /// </summary>
            [JsonProperty("normal")]
            public string Normal { get; set; }

            /// <summary>
            /// The path of the heightmap texture.
            /// </summary>
            [JsonProperty("heightmap")]
            public string Heightmap { get; set; }
        }
    }
}
