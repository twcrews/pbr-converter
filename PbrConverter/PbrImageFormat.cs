using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace Crews.Utility.PbrConverter
{
    /// <summary>
    /// Serves as type-safe enumerator and ImageFormat wrapper for use in PBR texture file formats.
    /// </summary>
    public class PbrImageFormat
    {
        /// <summary>
        /// String representation of the format.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// ImageFormat (if applicable) matching the format.
        /// </summary>
        public ImageFormat ImageFormat { get; private set; }

        /// <summary>
        /// TGA format.
        /// </summary>
        public static PbrImageFormat Tga => new PbrImageFormat("TGA", null);

        /// <summary>
        /// PNG format.
        /// </summary>
        public static PbrImageFormat Png => new PbrImageFormat("PNG", ImageFormat.Png);

        /// <summary>
        /// JPEG format.
        /// </summary>
        public static PbrImageFormat Jpeg => new PbrImageFormat("JPEG", ImageFormat.Jpeg);

        private PbrImageFormat(string val, ImageFormat format)
        {
            Value = val;
            ImageFormat = format;
        }

        public override bool Equals(object obj)
        {
            if (obj is ImageFormat)
            {
                return obj as ImageFormat == ImageFormat;
            }
            if (obj is PbrImageFormat)
            {
                return (obj as PbrImageFormat).Value == Value;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;
    }
}
