﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;

namespace PbrConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Path of the resource pack.
        /// </summary>
        public string Path { get => PathTextBox.Text; set => PathTextBox.Text = value; }

        /// <summary>
        /// Indicates whether the Path area and browse button are enabled.
        /// </summary>
        public bool PathEnabled { get => PathDockPanel.IsEnabled; set => PathDockPanel.IsEnabled = value; }

        /// <summary>
        /// The text to display on the Action button.
        /// </summary>
        public string ActionText { get => ActionButton.Content.ToString(); set => ActionButton.Content = value; }

        /// <summary>
        /// Indicates whether the Action button will be enabled.
        /// </summary>
        public bool ActionEnabled { get => ActionButton.IsEnabled; set => ActionButton.IsEnabled = value; }

        /// <summary>
        /// The text to display in the status text block.
        /// </summary>
        public string StatusText { get => StatusTextBlock.Text; set => StatusTextBlock.Text = value; }

        /// <summary>
        /// Indicates whether textures should be optimized.
        /// </summary>
        public bool Optimize { get => OptimizeCheckBox.IsChecked == true; set => OptimizeCheckBox.IsChecked = value; }

        /// <summary>
        /// Indicates whether uniform textures should be replaced with JSON values.
        /// </summary>
        public bool RemoveUniform { get => RemoveUniformCheckBox.IsChecked == true; set => RemoveUniformCheckBox.IsChecked = value; }

        /// <summary>
        /// Event raised when action button is clicked.
        /// </summary>
        public event EventHandler ActionClick;

        /// <summary>
        /// Event raised when path is changed.
        /// </summary>
        public event EventHandler PathChanged;

        /// <summary>
        /// Window constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e) => ActionClick.Invoke(this, e);

        private void PathTextBox_TextChanged(object sender, RoutedEventArgs e) => PathChanged.Invoke(this, e);

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                @"\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\resource_packs"
            };
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Path = fileDialog.FileName;
            }
        }
    }
}
