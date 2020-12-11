using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Crews.Utility.PbrConverter.Models;
using PbrConverter;
using System.Linq;
using System.Threading.Tasks;

namespace Crews.Utility.PbrConverter
{
    /// <summary>
    /// Contains functions related to resource packs.
    /// </summary>
    public static class ResourcePack
    {
        /// <summary>
        /// Retrieves the name of a given resource pack.
        /// </summary>
        /// <param name="path">The root path of the resource pack.</param>
        /// <returns>Returns a string representing the name of the pack.</returns>
        public static string GetName(string path)
        {
            string manifestName = JsonConvert.DeserializeObject<ManifestModel>(
                File.ReadAllText(path + @"\manifest.json")).Header.Name;

            if (manifestName == "pack.name")
            {
                // Get localized pack name.
                List<string> languages = JsonConvert.DeserializeObject<List<string>>(
                    File.ReadAllText(path + @"\texts\languages.json"));
                string currentCulture = CultureInfo.CurrentCulture.Name.Replace('-', '_');
                string langName = languages.Contains(currentCulture) ? 
                    currentCulture : languages.First();
                string langFileText = File.ReadAllText(path + @"\texts\" + langName + ".lang");
                return langFileText.Split("\n")[0].Split("=")[1].Trim();
            }

            return manifestName;
        }

        /// <summary>
        /// Retrieves all paths of color files (non-normal and non-mer) for a directory and all 
        /// subdirectories.
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

            Parallel.ForEach(Directory.GetFiles(path, "*.*", (SearchOption)sub), (filename) =>
            {
                if (IsColorFile(filename))
                {
                    returnList.Add(filename);
                }
            });
            return returnList;
        }

        /// <summary>
        /// Gets the PBR file name associated with an existing color file, if it exists.
        /// </summary>
        /// <param name="colorFilePath">The color file's path.</param>
        /// <param name="pbrType">The PBR file type to retrieve.</param>
        /// <returns>
        /// Returns a string representing the PBR file name if it exists, or null if it doesn't.
        /// </returns>
        public static string GetPbrFile(string colorFilePath, PbrType pbrType)
        {
            string pbrFileSuffix;

            switch (pbrType)
            {
                case PbrType.MER:
                    pbrFileSuffix = "_mer";
                    break;
                case PbrType.Normal:
                    pbrFileSuffix = "_normal";
                    break;
                case PbrType.Heightmap:
                    pbrFileSuffix = "_heightmap";
                    break;
                default:
                    return null;
            }

            string pbrFilePath = Path.GetDirectoryName(colorFilePath) + Path.DirectorySeparatorChar
                + Path.GetFileNameWithoutExtension(colorFilePath) + pbrFileSuffix;

            string[] matchingFiles = Directory.GetFiles(Path.GetDirectoryName(colorFilePath),
                Path.GetFileName(pbrFilePath) + ".*");

            if (matchingFiles.Length > 0)
            {
                return matchingFiles[0];
            }
            return null;
        }

        private static bool IsColorFile(string filename)
        {
            if (IsTexture(filename))
            {
                string noExtFile = Path.GetFileNameWithoutExtension(filename);
                return App.Configuration.AppData.Textures.Contains(noExtFile);
            }
            return false;
        }

        private static bool IsTexture(string path) =>
            PbrImageFormat.SupportedExtensions.Contains(Path.GetExtension(path).ToUpper());
    }

    /// <summary>
    /// PBR Type enumerable.
    /// </summary>
    public enum PbrType
    {
        /// <summary>
        /// Represents a color PBR texture.
        /// </summary>
        Color,

        /// <summary>
        /// Represents a MER PBR texture.
        /// </summary>
        MER,

        /// <summary>
        /// Represents a Normal PBR texture.
        /// </summary>
        Normal,

        /// <summary>
        /// Represents a heightmap PBR texture.
        /// </summary>
        Heightmap
    }
}
