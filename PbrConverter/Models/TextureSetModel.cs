using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crews.Utility.PbrConverter.Models
{
    public class TextureSetModel
    {
        [JsonProperty("format_version")]
        public string FormatVersion { get; set; }
        [JsonProperty("minecraft:texture_set")]
        public TextureSetInfo MinecraftTextureSet { get; set; }

        public class TextureSetInfo
        {
            [JsonProperty("color")]
            public object Color { get; set; }
            [JsonProperty("metalness_emissive_roughness")]
            public object MER { get; set; }
            [JsonProperty("normal")]
            public object Normal { get; set; }
        }
    }
}
