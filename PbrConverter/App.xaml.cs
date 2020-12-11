using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Crews.Utility.PbrConverter;
using Crews.Utility.PbrConverter.Models;
using Newtonsoft.Json;

namespace PbrConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow Window { get; set; }
        public static ConfigurationModel Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Configuration = JsonConvert.DeserializeObject<ConfigurationModel>(
                new StreamReader(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Crews.Utility.PbrConverter.settings.json"))
                .ReadToEnd());

            Window = new MainWindow
            {
                ActionText = "CONVERT",
                ActionEnabled = false,
                StatusText = "No valid resource pack selected."
            };
            Window.PathChanged += HandlePathChanged;
            Window.ActionClick += HandleActionButtonClick;
            Window.Show();
        }

        private void HandlePathChanged(object sender, EventArgs e)
        {
            try
            {
                Window.StatusText = "Successfully loaded \"" +
                    ResourcePack.GetName(Window.Path) + "\".";
                Window.ActionEnabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Window.StatusText = "No valid resource pack selected.";
                Window.ActionEnabled = false;
            }
        }

        private async void HandleActionButtonClick(object sender, EventArgs e)
        {
            Window.ActionEnabled = false;
            Window.PathEnabled = false;
            Window.StatusText = "Converting...";
            string pathCopy = Window.Path;
            PbrImageFormat formatCopy = Window.ImageFormat;
            bool removeUniformCopy = Window.RemoveUniform;
            await Task.Run(() => Convert(pathCopy, formatCopy, removeUniformCopy));
            Window.StatusText = "Successfully converted resource pack!";
            Window.ActionEnabled = true;
            Window.PathEnabled = true;
        }

        private void Convert(string path, PbrImageFormat format, bool removeSolids)
        {
            try
            {
                List<string> paths = ResourcePack.GetColorFiles(path + @"\textures\blocks");
                Parallel.ForEach(paths, (colorfile) =>
                {
                    string colorFileDir = Path.GetDirectoryName(colorfile) +
                        Path.DirectorySeparatorChar;

                    string colorfilename = Path.GetFileNameWithoutExtension(colorfile);

                    string merfile = ResourcePack.GetPbrFile(colorfile, PbrType.MER);
                    string normalfile = ResourcePack.GetPbrFile(colorfile, PbrType.Normal);
                    string heightmapfile = ResourcePack.GetPbrFile(colorfile, PbrType.Heightmap);

                    Texture colorTexture = new Texture(colorfile);
                    Texture merTexture = merfile != null ? new Texture(merfile) : new Texture();
                    Texture normalTexture = normalfile != null ? new Texture(normalfile) :
                        new Texture();
                    Texture heightmapTexture = heightmapfile != null ?
                        new Texture(heightmapfile) : new Texture();

                    Dictionary<Texture, object> textures = new Dictionary<Texture, object>
                    {
                        {colorTexture, colorfile },
                        {merTexture, merfile },
                        {normalTexture, normalfile },
                        {heightmapTexture, heightmapfile }
                    };
                    List<Texture> texCopy = new List<Texture>(textures.Keys);

                    if (removeSolids)
                    {
                        Parallel.ForEach(texCopy, (texture) =>
                        {
                        if (textures[texture] as string != normalfile && 
                            textures[texture] as string != heightmapfile &&
                            texture.Bitmap != null && texture.Uniform)
                            {
                                File.Delete(textures[texture] as string);
                                textures[texture] = textures[texture] as string == colorfile ?
                                texture.FullColorArray : texture.SolidColorArray;
                            }
                        });
                    }

                    if (format != null)
                    {
                        Parallel.ForEach(texCopy, (texture) =>
                        {
                            if (texture.Bitmap != null && textures[texture] is string)
                            {
                                if (File.Exists(textures[texture] as string))
                                {
                                    File.Delete(textures[texture] as string);
                                }
                                string newFile = Path.GetDirectoryName(
                                    textures[texture] as string) + Path.DirectorySeparatorChar +
                                    Path.GetFileNameWithoutExtension(
                                        textures[texture] as string) + "." + format.Value;
                                texture.Save(newFile, format);
                                textures[texture] = newFile;
                            }
                        });
                    }

                    Parallel.ForEach(texCopy, (texture) =>
                    {
                        if (textures[texture] != null)
                        {
                            if (textures[texture] is string)
                            {
                                textures[texture] = Path.GetFileName(textures[texture] as string);
                            }
                        }
                    });

                    using FileStream fs = File.Create(Path.GetDirectoryName(colorfile) + @"\" +
                        colorfilename + ".texture_set.json");
                    byte[] content = new UTF8Encoding(true).GetBytes(
                        JsonConvert.SerializeObject(new TextureSetModel
                        {
                            FormatVersion = Configuration.AppData.TextureSetVersion,
                            MinecraftTextureSet = new TextureSetModel.TextureSetInfo
                            {
                                Color = textures[colorTexture],
                                MER = textures[merTexture],
                                Normal = textures[normalTexture] as string,
                                Heightmap = textures[heightmapTexture] as string
                            }
                        }, Formatting.Indented, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }));
                    fs.Write(content);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error during conversion.", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
