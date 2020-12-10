using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        /// The display string for a solid color value in JSON array format.
        /// </summary>
        public string DisplayValue => ToArrayString(Value);

        /// <summary>
        /// 
        /// </summary>
        public bool Solid => IsSolid();

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

        private bool IsSolid()
        {
            int width = Bitmap.Width;
            int height = Bitmap.Height;

            Rectangle rectangle = new Rectangle(0, 0, width, height);
            int depth = Image.GetPixelFormatSize(Bitmap.PixelFormat);
            if (depth != 8 && depth != 24 && depth != 32)
            {
                throw new ArgumentException("Only 8, 24, and 32-bit images are supported.");
            }

            BitmapData data = Bitmap.LockBits(
                rectangle, ImageLockMode.ReadOnly, Bitmap.PixelFormat);
            int step = depth / 8;
            byte[] pixels = new byte[width * height * step];

            IntPtr iptr = data.Scan0;
            Marshal.Copy(iptr, pixels, 0, pixels.Length);
            Bitmap.UnlockBits(data);

            Color firstColor = GetPixel(0, 0, width, step, pixels);
            bool difference = false;
            if (height > 1)
            {
                Parallel.For(0, height, (y, loopState) =>
                {
                    if (width > 1)
                    {
                        Parallel.For(1, width, (x, loopState) =>
                        {
                            Color color = GetPixel(x, y, width, step, pixels);
                            if (color != firstColor)
                            {
                                difference = true;
                                loopState.Stop();
                            }
                        });
                    }
                    else
                    {
                        Color color = GetPixel(0, y, 1, step, pixels);
                        if (color != firstColor)
                        {
                            difference = true;
                        }
                    }
                    if (difference)
                    {
                        loopState.Stop();
                    }
                });
            }
            else
            {
                if (width > 1)
                {
                    Parallel.For(1, width, (x, loopState) =>
                    {
                        Color color = GetPixel(x, 0, width, step, pixels);
                        if (color != firstColor)
                        {
                            difference = true;
                            loopState.Stop();
                        }
                    });
                }
            }
            if (difference)
            {
                return false;
            }
            return true;
        }

        private static Color GetPixel(int x, int y, int width, int step, byte[] pixels)
        {
            int i = (y * width + x) * step;
            return step switch
            {
                4 => Color.FromArgb(pixels[i + 3], pixels[i + 2], pixels[i + 1], pixels[i]),
                3 => Color.FromArgb(pixels[i + 2], pixels[i + 1], pixels[i]),
                1 => Color.FromArgb(pixels[i], pixels[i], pixels[i]),
                _ => throw new ArgumentException("Invalid byte step; must be 1, 3, or 4.")
            };
        }

        private static string ToArrayString(Color color) => "[ " + color.R.ToString() + ", " + 
            color.G.ToString() + ", " + color.B.ToString() + ", " + color.A.ToString() + " ]";
    }
}
