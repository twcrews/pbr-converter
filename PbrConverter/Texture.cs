using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Crews.Utility.PbrConverter
{
    /// <summary>
    /// Represents texture data.
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Image representation of the texture.
        /// </summary>
        public Bitmap Bitmap { get; set; }

        /// <summary>
        /// 4-channel representation of the texture based on the first pixel.
        /// </summary>
        public Color Value => Bitmap.GetPixel(0, 0);

        /// <summary>
        /// Instantiate a new Texture from a Bitmap.
        /// </summary>
        /// <param name="bitmap">A bitmap image object.</param>
        public Texture(Bitmap bitmap) => Bitmap = bitmap;

        /// <summary>
        /// Instantiate a new Texture from a Color.
        /// </summary>
        /// <param name="color">A 4-channel color object.</param>
        public Texture(Color color)
        {
            Bitmap bmp = new Bitmap(1, 1);
            bmp.SetPixel(1, 1, color);
            Bitmap = bmp;
        }
        
        /// <summary>
        /// Instantiate a new Texture from an image file path.
        /// </summary>
        /// <param name="path">The path of an image file.</param>
        public Texture(string path)
        {
            if (Path.GetExtension(path).ToUpper() == ".TGA")
            {
                Bitmap = TgaSharp.TGA.FromFile(path).ToBitmap();
            } 
            else
            {
                Bitmap = new Bitmap(path);
            }
        }

        /// <summary>
        /// Saves the texture Bitmap to a file.
        /// </summary>
        /// <param name="path">The path to save to.</param>
        /// <param name="format">The format of the image to save.</param>
        public void Save(string path, PbrImageFormat format)
        {
            if (format == PbrImageFormat.Tga)
            {
                TgaSharp.TGA.FromBitmap(Bitmap).Save(path);
            }
            else
            {
                Bitmap.Save(path, format.ImageFormat);
            }
        }
    }
}
