using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Drawing.Common;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] imagePaths = { "image1.jpg", "image2.jpg", "image3.jpg" }; // список шляхів до зображень

            Func<Bitmap, Bitmap> grayscale = image =>
            {
                Bitmap grayscaleImage = new Bitmap(image.Width, image.Height);

                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        Color pixelColor = image.GetPixel(x, y);
                        int grayscaleValue = (int)(0.2989 * pixelColor.R + 0.5870 * pixelColor.G + 0.1140 * pixelColor.B);
                        Color grayscaleColor = Color.FromArgb(pixelColor.A, grayscaleValue, grayscaleValue, grayscaleValue);
                        grayscaleImage.SetPixel(x, y, grayscaleColor);
                    }
                }

                return grayscaleImage;
            };

            Func<Bitmap, Bitmap> invertColors = image =>
            {
                Bitmap invertedImage = new Bitmap(image.Width, image.Height);

                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        Color pixelColor = image.GetPixel(x, y);
                        Color invertedColor = Color.FromArgb(pixelColor.A, 255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                        invertedImage.SetPixel(x, y, invertedColor);
                    }
                }

                return invertedImage;
            };

            Action<Bitmap> displayImage = image =>
            {
                image.Save(Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".jpg"); // зберігаємо оброблене зображення в новому файлі з випадковим іменем
                Console.WriteLine("Image processed and saved to disk");
            };

            var processedImages = imagePaths.Select(path => new Bitmap(path))
                                            .Select(grayscale)
                                            .Select(invertColors);

            foreach (var image in processedImages)
            {
                displayImage(image);
            }
        }
    }
}
