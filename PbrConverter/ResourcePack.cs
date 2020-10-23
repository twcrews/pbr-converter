using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Media.Media3D;
using Crews.Utility.PbrConverter.Models;
using PbrConverter;
using System.Linq;

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
                string langName = languages.Contains(currentCulture) ? currentCulture : languages.First();
                string langFileText = File.ReadAllText(path + @"\texts\" + langName + ".lang");
                return langFileText.Split("\n")[0].Split("=")[1].Trim();
            }

            return manifestName;
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
                default:
                    return null;
            }

            string pbrFilePath = Path.GetDirectoryName(colorFilePath) + @"\" +
                Path.GetFileNameWithoutExtension(colorFilePath) + pbrFileSuffix;

            string[] matchingFiles = Directory.GetFiles(Path.GetDirectoryName(colorFilePath),
                Path.GetFileName(pbrFilePath) + ".*");

            if (matchingFiles.Length > 0)
            {
                return Path.GetFileNameWithoutExtension(pbrFilePath);
            }
            return null;
        }

        private static bool IsColorFile(string filename)
        {
            List<string> supportedExtensions = new List<string> { ".TGA", ".PNG", ".JPG", ".JPEG" };
            if (supportedExtensions.Contains(Path.GetExtension(filename).ToUpper()))
            {
                string noExtFile = Path.GetFileNameWithoutExtension(filename);
                return App.Configuration.AppData.Blocks.Contains(noExtFile);
            }
            return false;
        }
    }

    public enum PbrType
    {
        Color,
        MER,
        Normal
    }
}
