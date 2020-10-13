using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace tp_images
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Bitmap imageOriginale = new Bitmap("image.jpg");
            List<Bitmap> images = new List<Bitmap>() { new Bitmap("image5.jpg"), new Bitmap("image10.jpg"), new Bitmap("image15.jpg"), new Bitmap("image20.jpg"), new Bitmap("image25.jpg"), new Bitmap("image30.jpg"), new Bitmap("image35.jpg"), new Bitmap("image40.jpg"), new Bitmap("image45.jpg"), new Bitmap("image50.jpg") };
            Console.WriteLine("image originale : " + NombreDeCouleur(imageOriginale) + " de couleurs");
            int qualite = 5;
            foreach (Bitmap b in images)
            {
                Console.WriteLine("image qualité " + qualite + " : PSNR = " + psnr(imageOriginale, b));
                qualite += 5;
            }
            
        }

        static void DecompositionRGB()
        {
            string image = "image.jpg";
            Bitmap bmp = new Bitmap(image);
            int width = bmp.Width;
            int height = bmp.Height;

            Bitmap redBMP = new Bitmap(bmp);
            Bitmap greenBMP = new Bitmap(bmp);
            Bitmap blueBmp = new Bitmap(bmp);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Color pixel = bmp.GetPixel(i,j);
                    int a = pixel.A;
                    int red = pixel.R;
                    int green = pixel.G;
                    int blue = pixel.B;

                    redBMP.SetPixel(i, j, Color.FromArgb(a, red, 0, 0));
                    
                    greenBMP.SetPixel(i, j, Color.FromArgb(a, 0, green, 0));
                    
                    blueBmp.SetPixel(i, j, Color.FromArgb(a, 0, 0, blue));
                    

                }
            }
            redBMP.Save(@"red.png");
            greenBMP.Save(@"green.png");
            blueBmp.Save(@"blue.png");
        }

        static int NombreDeCouleur(Bitmap bmp)
        {
            HashSet<Color> colors = new HashSet<Color>();   //liste qui n'accepte pas les doublons

            int width = bmp.Width;
            int height = bmp.Height;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    colors.Add(bmp.GetPixel(i, j));

                }
            }
            int nbColor = colors.Count;
            return nbColor;
        }

        static double psnr(Bitmap bmp, Bitmap compressedBMP)
        {
            double mse = 0;
            double redMse = 0;
            double greenMse = 0;
            double blueMse = 0;
            int width = bmp.Width;
            int height = bmp.Height;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    redMse += Math.Pow(bmp.GetPixel(i, j).R - compressedBMP.GetPixel(i,j).R, 2);
                    greenMse += Math.Pow(bmp.GetPixel(i, j).G - compressedBMP.GetPixel(i, j).G, 2);
                    blueMse += Math.Pow(bmp.GetPixel(i, j).B - compressedBMP.GetPixel(i, j).B, 2);
                }
            }
            mse += (redMse + blueMse + greenMse) / (width * height);
            double psnr = 10 * Math.Log(Math.Pow(255,2) / mse, 10);
            Console.WriteLine(mse);

            return psnr;
        }

        /// <summary>
        ///  Convert RGB image to HSV ( data store in a matrix)
        /// </summary>
        /// <returns></returns>
        static double[,][] RgbToHsv()
        {
            Bitmap bmp = new Bitmap("image.jpg");
            Bitmap bmpHSV = new Bitmap(bmp);
            int width = bmp.Width;
            int height = bmp.Height;
            double[,][] HSV = new double[width, height][];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Color color = bmp.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int[] RGB = new int[] { R, G, B };
                    double V = color.GetBrightness();
                    double S = color.GetSaturation();
                    double H = color.GetHue();
                    HSV[i, j] = new double[] { H, S, V };

                }
            }

            Console.WriteLine("Done");
            return HSV;
        }

        static void HsvToRgb(double[,][] HSV)
        {
            int width = HSV.GetLength(0);
            int height = HSV.GetLength(1);
            Bitmap bmpRGB = new Bitmap(width, height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    bmpRGB.SetPixel(i, j, ColorFromHSV(HSV[i,j]));

                }
            }
            bmpRGB.Save(@"rgbfromHSV.png");
            Console.WriteLine("Done");
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public static Color ColorFromHSV(double[] HSV)
        {
            double hue = HSV[0];
            double saturation = HSV[1];
            double value = HSV[2];

            int hue2 = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hue2 == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hue2 == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hue2 == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hue2 == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hue2 == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        /// <summary>
        ///  Convert RGB image to YUV ( data store in a matrix)
        /// </summary>
        /// <returns></returns>
        static double[,][] RGBtoYUV()
        {
            Bitmap bmp = new Bitmap("image.jpg");

            int width = bmp.Width;
            int height = bmp.Height;

            double[,][] bmpYUV = new double[width, height][];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Color color = bmp.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int[] RGB = new int[] { R, G, B };
                    double[] res = encodeYUV(RGB);
                    bmpYUV[i, j] = res;

                }
            }
            return bmpYUV;
        }

        /// <summary>
        ///  Convert YUV image (matrix) to RGB image 
        /// </summary>
        /// <returns></returns>
        static void YUVtoRGB(double[,][] YUV)
        {
            
            int width = YUV.GetLength(0);
            int height = YUV.GetLength(1);
            Bitmap bmpRGB = new Bitmap(width, height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    double[] res = encodeRGBfromYUV(YUV[i,j]);
                    bmpRGB.SetPixel(i, j, Color.FromArgb(Convert.ToInt32(res[0]), Convert.ToInt32(res[1]), Convert.ToInt32(res[2])));

                }
            }
            bmpRGB.Save(@"rgbfromyuv.png");
            Console.WriteLine("Done");
        }

        /// <summary>
        ///  Calculate YUV from RGB
        /// </summary>
        /// <returns></returns>
        static public double[] encodeYUV(int[] RGB)
        {
            double y = RGB[0] * .299000 + RGB[1] * .587000 + RGB[2] * .114000;
            double u = 0.493 * (RGB[2] - y);
            double v = 0.877 * (RGB[0] - y);

            double[] YUV = { y, u, v };

            return YUV;
        }


        /// <summary>
        ///  Calculate RGB from YUV
        /// </summary>
        /// <returns></returns>
        static public double[] encodeRGBfromYUV(double[] YUV)
        {
            double r = YUV[0] + 1.14 * YUV[2];
            double g = YUV[0] - 0.394 * YUV[1] - 0.581 * YUV[2];
            double b = YUV[0] + 2.032 * YUV[1];

            double[] RGB = {Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b) };

            return RGB;
        }

        static void exo4()
        {
            Bitmap imageOriginale = new Bitmap("image.jpg");
            List<Bitmap> images = new List<Bitmap>() { new Bitmap("image5.jpg"), new Bitmap("image10.jpg"), new Bitmap("image15.jpg"), new Bitmap("image20.jpg"), new Bitmap("image25.jpg"), new Bitmap("image30.jpg"), new Bitmap("image35.jpg"), new Bitmap("image40.jpg"), new Bitmap("image45.jpg"), new Bitmap("image50.jpg") };
            Console.WriteLine("image originale : " + NombreDeCouleur(imageOriginale) + " de couleurs");
            int qualite = 5;
            foreach (Bitmap b in images)
            {
                Console.WriteLine("image qualité " + qualite + " : PSNR = " + psnr(imageOriginale, b) + ", " + NombreDeCouleur(b) + " de couleurs");
                qualite += 5;
            }
        }

        /// <summary>
        /// Print program usage
        /// </summary>
        static void Usage()
        {
            Console.WriteLine("Usage: {0} file1 file2", Environment.GetCommandLineArgs()[0].ToLower());
            Console.WriteLine("   Computes the Structural Similarity Index Metric (SSIM) between the images.");
            Console.WriteLine("   SSIM = 1 is a perfect match, and values decrease to 0 as images diverge.");
            Console.WriteLine("   Supports at least jpg, bmp, png, and gif.");
            Console.WriteLine("   See the Wikipedia entry on SSIM and related links.");
            Console.WriteLine("   By Chris Lomont, www.Lomont.org, Version {0}, Sept 2009", SSIM.Version);
        }

        /// <summary>
        /// Try to get a bitmap image. Issue warnings and exit program if cannot. Return image or null
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static Bitmap GetImage(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("ERROR: File {0} does not exist", filename);
                    Environment.Exit(-2);
                }

                return new Bitmap(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File {0} not a supported image file\n", filename);
                Environment.Exit(-3);
            }
            return null;
        }

        static void ssim(Bitmap img1, Bitmap img2)
        {
            Console.WriteLine("ok1");
            try
            {
                var start = Environment.TickCount;
                
                if ((img1 == null) || (img2 == null))
                {
                    Console.WriteLine("Could not open image files {0}  and {1}\n", "image.jpg", "image25.jpg");
                    Environment.Exit(-3);
                }
                if ((img1.Width != img2.Width) || (img1.Height != img2.Height) || (img1.PixelFormat != img2.PixelFormat))
                {
                    Console.WriteLine("Images not same size or bitdepth\n");
                    Environment.Exit(-4);
                }
                if ((img1.Width < 11) || (img1.Height < 11))
                {
                    Console.WriteLine("Images too small for meaningful comparison\n");
                    Environment.Exit(-5);
                }

                // create new SSIM object and feed the images to it
                var ssim = new SSIM();
                var index = ssim.Index(img1, img2);
                Console.WriteLine("SSIM: {0}", index);
                var end = Environment.TickCount;
            }
            catch (InvalidCastException e) // todo - Make into Exception
            {
                Console.WriteLine("Unhandled Exception {0}", e.Message);
            }
        }

    }
}
