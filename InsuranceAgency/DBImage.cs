using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace InsuranceAgency
{
    public static class DBImage
    {
        public static string Encode(BitmapImage image)
        {
            string base64 = Convert.ToBase64String(CopyImageToByteArray(image));
            return base64;
        }
        private static byte[] CopyImageToByteArray(BitmapImage image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                ConvertToImage(image).Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        private static Image ConvertToImage(BitmapImage bitmapImage)
        {
            MemoryStream ms = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(ms);

            Bitmap bmp = new Bitmap(ms);
            Image image = bmp;
            return image;
        }



        public static BitmapImage Decode(string base64)
        {
            Image image = GetImageFromByteArray(base64);
            return ConvertToBitmapImage(image);
        }
        private static Image GetImageFromByteArray(string base64)
        {
            var image = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
            return image;
        }
        private static BitmapImage ConvertToBitmapImage(Image img)
        {
            BitmapImage bmImg = new BitmapImage();

            using (MemoryStream memStream2 = new MemoryStream())
            {
                img.Save(memStream2, System.Drawing.Imaging.ImageFormat.Png);
                memStream2.Position = 0;

                bmImg.BeginInit();
                bmImg.CacheOption = BitmapCacheOption.OnLoad;
                bmImg.UriSource = null;
                bmImg.StreamSource = memStream2;
                bmImg.EndInit();
            }

            return bmImg;
        }
        
    } 
}
