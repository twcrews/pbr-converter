using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Media.Media3D;

namespace Crews.Utility.PbrConverter
{
    /// <summary>
    /// Contains functions related to resource packs.
    /// </summary>
    public class ResourcePack
    {
        /// <summary>
        /// Retrieves the name of a given resource pack.
        /// </summary>
        /// <param name="path">The root path of the resource pack.</param>
        /// <returns>Returns a string representing the name of the pack.</returns>
        public static string GetName(string path) =>
            JsonConvert.DeserializeObject<Manifest>(
                File.ReadAllText(path + @"\manifest.json")).Header.Name;

        /// <summary>
        /// Retrieves a TextureSet object with default values based on the provided colorFile name.
        /// </summary>
        /// <param name="colorFile">The name of the color file (without extension or path).</param>
        /// <returns>Returns a TextureSet object.</returns>
        public static TextureSet GenerateTextureSet(string colorFile) =>
            GenerateTextureSet("1.16.100", colorFile, colorFile + "_mer", colorFile + "_normal");

        /// <summary>
        /// Retrieves a TextureSet object with the provided values.
        /// </summary>
        /// <param name="formatVersion">The format_version property of the texture set.</param>
        /// <param name="color">The color file name or rbg/rgba value of the texture set.</param>
        /// <param name="mer">The metalness/emissive/roughness file name or rbg/rgba value of the texture set.</param>
        /// <param name="normal">The normal file name or rbg/rgba value of the texture set.</param>
        /// <returns></returns>
        public static TextureSet GenerateTextureSet(string formatVersion, object color, object mer, object normal)
        {
            return new TextureSet
            {
                FormatVersion = formatVersion,
                MinecraftTextureSet = new TextureSet.TextureSetInfo
                {
                    Color = color,
                    MER = mer,
                    Normal = normal
                }
            };
        }

        /// <summary>
        /// Retrieves all paths of color files (non-normal and non-mer) for a directory and all subdirectories.
        /// </summary>
        /// <param name="path">The base path of the texture files.</param>
        /// <returns></returns>
        public static List<string> GetColorFiles(string path) => GetColorFiles(path, true);

        /// <summary>
        /// Retrieves all paths of color files (non-normal and non-mer) for a directory.
        /// </summary>
        /// <param name="path">The base path of the texture files.</param>
        /// <param name="subdirectories">Indicates whether to search subdirectories.</param>
        /// <returns>Returns a list of strings representing color file paths.</returns>
        public static List<string> GetColorFiles(string path, bool subdirectories)
        {
            List<string> returnList = new List<string>();
            int sub = Convert.ToInt32(subdirectories);

            foreach (string filename in Directory.GetFiles(path, "*.*", (SearchOption)sub))
            {
                if (IsColorFile(filename))
                {
                    returnList.Add(filename);
                }
            }

            return returnList;
        }

        private static bool IsColorFile(string filename) {
            List<string> supportedExtensions = new List<string> { ".TGA", ".PNG", ".JPG", ".JPEG" };
            if (supportedExtensions.Contains(Path.GetExtension(filename).ToUpper()))
            {
                string noExtFile = Path.GetFileNameWithoutExtension(filename);
                return !noExtFile.EndsWith("_mer") && !noExtFile.EndsWith("_normal");
            }
            return false;
        }

        /// <summary>
        /// Model used to represent a manifest.json file.
        /// </summary>
        public class Manifest
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

        /// <summary>
        /// Model used to represent a *.texture_set.json file.
        /// </summary>
        public class TextureSet
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
}
