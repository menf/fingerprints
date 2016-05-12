using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Zmienne

        
        public static Bitmap bmpOut = null;
        private static Bitmap bmpGray = null;
    
        private static int gray = 0;
        BitmapSource bmpSource = null;
        byte[] pixelValues;
        byte[] skeleton;
        #endregion


        public MainWindow()
        {
            InitializeComponent();
        }

        #region Plik

        public ImageSource BitmapToImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }


        private void Wczytaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Wybierz plik";
            op.Filter = "All Files (*.*)|*.*|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif;*.tiff)|*.tif;*.tiff|BMP (.bmp)|*bmp";
            if (op.ShowDialog() == true)
            {


                bmpSource = new BitmapImage(new Uri(op.FileName));
                using (MemoryStream ms = new MemoryStream())
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)bmpSource));
                    encoder.Save(ms);

                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        bmpOut = new Bitmap(bmp);
                    
                        
                    }
                }
                
              
            
                image.Source = BitmapToImage(bmpOut);

                

            }
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            if (image.Source == null)
            {
                System.Windows.MessageBox.Show("Aby zapisać, wczytaj obraz.", "Brak obrazu!");

            }
            else if (image.Source != null)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Title = "Zapisywanie jako";
                save.Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|TIFF (*.tif;*.tiff)|*.tif;*.tiff";
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // ImageFormat format = ImageFormat.Bmp;
                BitmapEncoder encoder;

                if (save.ShowDialog() == true)
                {
                    string ext = System.IO.Path.GetExtension(save.FileName);
                    FileStream filestream = new FileStream(save.FileName, FileMode.Create);
                    switch (ext)
                    {
                        case ".jpg":
                            //format = ImageFormat.Jpeg;
                            encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".jpeg":
                            // format = ImageFormat.Jpeg;
                            encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".bmp":
                            //format = ImageFormat.Bmp;
                            encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".png":
                            //format = ImageFormat.Png;
                            encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".gif":
                            // format = ImageFormat.Gif;
                            encoder = new GifBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".tif":
                            //  format = ImageFormat.Tiff;
                            encoder = new TiffBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                        case ".tiff":
                            // format = ImageFormat.Tiff;
                            encoder = new TiffBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                            encoder.Save(filestream);
                            break;
                    }
                    filestream.Close();
                }
            }
        }


      private void Exit_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();

        }
       

        #endregion


  

        #region Szarosc


        private Bitmap ToGray(Bitmap bmp)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            
            g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
               0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }


        private void Szarosc_Click(object sender, RoutedEventArgs e)
        {
            FormatConvertedBitmap format = new FormatConvertedBitmap();
            format.BeginInit();
            format.Source = bmpSource;
            format.DestinationFormat = PixelFormats.Gray8;
            format.EndInit();
            bmpSource = format;
            image.Source = format;
            using (MemoryStream ms1 = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                encoder.Save(ms1);

                using (Bitmap bmp = new Bitmap(ms1))
                {
                    bmpOut = new Bitmap(bmp);
           
                }
            }
        }

        #endregion
        

        #region Binaryzacja


        #region Otsu


        public Bitmap ImageToBitmap(BitmapSource bitmapSource)
        {
            if (bitmapSource != null)
            {
                var width = bitmapSource.PixelWidth;
                var height = bitmapSource.PixelHeight;
                var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
                var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
                bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
                var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, memoryBlockPointer);
                return bitmap;
            }
            else
                return null;
        }


        public int OtsuThreshold()
        {
            Bitmap bmp = ImageToBitmap((BitmapSource)image.Source);
            int[] histogram = new int[256];
            for (int i = 0; i < histogram.Length; i++) histogram[i] = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    System.Drawing.Color c = bmp.GetPixel(i, j);
                    int value = (c.R + c.G + c.B) / 3;
                    histogram[value]++;
                }
            }
            int total = bmp.Height * bmp.Width;

            float sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += i * histogram[i];
            }

            float sumB = 0;
            int wB = 0;
            int wF = 0;

            float varMax = 0;
            int threshold = 0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0) continue;
                wF = total - wB;

                if (wF == 0) break;

                sumB += (float)(i * histogram[i]);
                float mB = sumB / wB;
                float mF = (sum - sumB) / wF;

                float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = i;
                }
            }
            return threshold;
        }

        private void Otsu_Click(object sender, RoutedEventArgs e)
        {

            if (bmpOut != null)
            {
                if (gray == 0)
                {
                    bmpOut = ToGray(bmpOut);
                }

                int a = OtsuThreshold();
                byte[] LUT = new byte[256];

                Bitmap bitmap = bmpOut;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];

                for (int i = 0; i < a; i++) LUT[i] = 0;
                for (int i = a; i < 256; i++) LUT[i] = 255;

                Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
                for (int i = 0; i < pixelValues.Length; i++)
                {
                    pixelValues[i] = LUT[pixelValues[i]];
                }
                Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
                bitmap.UnlockBits(bmpData);
                image.Source = BitmapToImage(bitmap);


            }
        }
        #endregion

        #region Ręcznie

        private void Progowanie_Click(object sender, RoutedEventArgs e)
        {
            if (bmpOut != null)
            {
                if (gray == 0)
                {
                    bmpOut = ToGray(bmpOut);
                }

                int a = int.Parse(Bin_Prog.Text);
                byte[] LUT = new byte[256];

                Bitmap bitmap = bmpOut;
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bitmap.Height];

                for (int i = 0; i < a; i++) LUT[i] = 0;
                for (int i = a; i < 256; i++) LUT[i] = 255;

                Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);
                for (int i = 0; i < pixelValues.Length; i++)
                {
                    pixelValues[i] = LUT[pixelValues[i]];
                }
                Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
                bitmap.UnlockBits(bmpData);
                image.Source = BitmapToImage(bitmap);
            }
        }
        #endregion

       
        #endregion


        #region Scienianie
        private void scienianie_Click(object sender, RoutedEventArgs e)
        {

                 FormatConvertedBitmap format = new FormatConvertedBitmap();
                  format.BeginInit();
                  format.Source = bmpSource;
                  format.DestinationFormat = PixelFormats.Gray8;
                  format.EndInit();
                  bmpSource = format;
                  image.Source = format;
                  using (MemoryStream ms1 = new MemoryStream())
                  {
                      PngBitmapEncoder encoder = new PngBitmapEncoder();
                      encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                      encoder.Save(ms1);

                      using (Bitmap bmp = new Bitmap(ms1))
                      {
                          bmpOut = new Bitmap(bmp);

                      }
                  }
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmpOut.Width, bmpOut.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmpOut.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            pixelValues = new byte[Math.Abs(bmpData.Stride) * bmpOut.Height];
            Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);

            int height = bmpSource.PixelHeight;
            int width = bmpSource.PixelWidth;




           skeleton = new byte[pixelValues.Length];
            for (int x = 0; x < width * height; x++)
            {

                if (pixelValues[x] == 0)
                {
                    skeleton[x] = 1;
                }
                else skeleton[x] = 0;
            }
            // etap 3

            for(int i=0;i<10;i++)
            {
            for (int x = width + 1; x < (width * height) - width - 1; x++)
            {
                if ((skeleton[x - 1] == 0 || skeleton[x + 1] == 0 || skeleton[x - width] == 0 || skeleton[x + width] == 0) && skeleton[x] == 1) skeleton[x] = 2;
                else if ((skeleton[x - width - 1] == 0 || skeleton[x - width + 1] == 0 || skeleton[x + width - 1] == 0 || skeleton[x + width + 1] == 0) && skeleton[x] == 1) skeleton[x] = 3;
            }
            // etap 4
            int[] neighborhoodCheck = { 3, 12, 48, 192, 6, 24, 96, 129, // - 2 sąsiadów
                                        14, 56, 131, 224, 7, 28, 112, 193, // - 3 sąsiadów
                                        195, 135, 15, 30, 60, 120, 240, 225}; // - 4 sąsiadów

            for (int x = width + 1; x < (width * height) - width - 1; x++)
            {
                int wynik = 0;
                if (skeleton[x] == 2)
                {
                    if (skeleton[x - width] != 0) wynik += 1;
                    if (skeleton[x - width + 1] != 0) wynik += 2;
                    if (skeleton[x + 1] != 0) wynik += 4;
                    if (skeleton[x + 1 + width] != 0) wynik += 8;
                    if (skeleton[x + width] != 0) wynik += 16;
                    if (skeleton[x + width - 1] != 0) wynik += 32;
                    if (skeleton[x - 1] != 0) wynik += 64;
                    if (skeleton[x - 1 - width] != 0) wynik += 128;
                }
                if (neighborhoodCheck.Contains(wynik))
                {
                    skeleton[x] = 4;

                }
            }


          
            
            
                for (int x = width; x < (width * height) - width; x++)
                {
                    if (skeleton[x] == 4)
                        skeleton[x] = 0;
                }
            
            




            //etap 6
            int[] deleteArray = {5, 13, 15, 20, 21, 22, 23, 29,
                                30, 31, 52, 53, 54, 55, 60, 61,
                                62, 63, 65, 67, 69, 71, 77, 79,
                                80, 81, 83, 84, 85, 86, 87, 88,
                                89, 91, 92, 93, 94, 95, 97, 99,
                                101, 103, 109, 111, 113, 115, 116, 117,
                                118, 119, 120, 121, 123, 124, 125, 126,
                                127, 133, 135, 141, 143, 149, 151, 157,
                                159, 181, 183, 189, 191, 195, 197, 199,
                                205, 207, 208, 209, 211, 212, 213, 214,
                                215, 216, 217, 219, 220, 221, 222, 223,
                                225, 227, 229, 231, 237, 239, 240, 241,
                                243, 244, 245, 246, 247, 248, 249, 251,
                                252, 253, 254, 255,
                                3, 12, 48, 192,
                                14, 56, 131, 224,
                                7, 28, 112, 193
                                };
            for (int x = width + 1; x < (width * height )- width - 1; x++)
            {
                int wynik = 0;
                if (skeleton[x] == 2)
                {
                    if (skeleton[x - width] != 0) wynik += 1;
                    if (skeleton[x - width + 1] != 0) wynik += 2;
                    if (skeleton[x + 1] != 0) wynik += 4;
                    if (skeleton[x + 1 + width] != 0) wynik += 8;
                    if (skeleton[x + width] != 0) wynik += 16;
                    if (skeleton[x + width - 1] != 0) wynik += 32;
                    if (skeleton[x - 1] != 0) wynik += 64;
                    if (skeleton[x - 1 - width] != 0) wynik += 128;
                }
                if (deleteArray.Contains(wynik))
                {
                    skeleton[x] = 0;
                }
            }


            for (int x = width + 1; x < (width * height) - width - 1; x++)
            {
                int wynik = 0;
                if (skeleton[x] == 3)
                {
                    if (skeleton[x - width] != 0) wynik += 1;
                    if (skeleton[x - width + 1] != 0) wynik += 2;
                    if (skeleton[x + 1] != 0) wynik += 4;
                    if (skeleton[x + 1 + width] != 0) wynik += 8;
                    if (skeleton[x + width] != 0) wynik += 16;
                    if (skeleton[x + width - 1] != 0) wynik += 32;
                    if (skeleton[x - 1] != 0) wynik += 64;
                    if (skeleton[x - 1 - width] != 0) wynik += 128;
                }
                if (deleteArray.Contains(wynik))
                {
                    skeleton[x] = 0;
                }
            }

            //etap 8
            for (int x = 0; x < width * height; x++)
            {

                if (skeleton[x] != 0)
                {
                    skeleton[x] = 1;
                }
            }



        }
        
            for (int x = 0; x < width * height; x++)
            {

                if (skeleton[x] == 1 )
                {
                    skeleton[x] = 0;
                }
                else skeleton[x] = 255;
            }
            Marshal.Copy(skeleton, 0, bmpData.Scan0, skeleton.Length);
            bmpOut.UnlockBits(bmpData);
            image.Source = BitmapToImage(bmpOut);
            bmpSource = (BitmapSource)image.Source;



           

            /* image.Source = BitmapToImage(bmpOut);
             using (MemoryStream ms = new MemoryStream())
             {
                 PngBitmapEncoder encoder = new PngBitmapEncoder();
                 encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
                 encoder.Save(ms);

                 using (Bitmap bmp = new Bitmap(ms))
                 {
                     bmpOut = new Bitmap(bmp);
                 }

             }
             */
            /* System.Windows.Media.PixelFormat format = new System.Windows.Media.PixelFormat();
             format = PixelFormats.Gray8;

             BitmapPalette paleta = new BitmapPalette(bmpSource, 256);
             double ppiX = 300.0;
             double ppiY = 600.0; 

           image.Source = BitmapSource.Create(width, height, ppiX, ppiY, format, paleta, skeleton, width);
             */
        }
        #endregion

    }
}

