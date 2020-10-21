using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Crews.Utility.PbrConverter;
using Newtonsoft.Json;

namespace PbrConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow Window { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
                Window.StatusText = "Successfully loaded \"" + ResourcePack.GetName(Window.Path) + "\".";
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
            await Task.Run(() => Convert(pathCopy));
            Window.StatusText = "Successfully converted resource pack!";
            Window.ActionEnabled = true;
            Window.PathEnabled = true;
        }

        private void Convert(string path)
        {
            try
            {
                List<string> paths = ResourcePack.GetColorFiles(path);
                foreach (string colorfile in paths)
                {
                    string colorfilename = Path.GetFileNameWithoutExtension(colorfile);
                    using (FileStream fs = File.Create(Path.GetDirectoryName(colorfile) + @"\" +
                        colorfilename + ".texture_set.json"))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(
                            JsonConvert.SerializeObject(ResourcePack.GenerateTextureSet(colorfilename)));
                        fs.Write(content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error during conversion.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
