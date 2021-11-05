using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebCamLib;
using ImageProcess2; 

namespace Villacarlos_ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loadimage, resultimage;
        Device[] myDevices; //the device you wanted to use as a webcam 

        public Form1()
        {
            InitializeComponent();
        }

        //Loading an image
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(); //open an open file dialog that once clicked would load an image to the app
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadimage = new Bitmap(openFileDialog1.FileName); //retrieve an image
            pictureBox1.Image = loadimage; //update the picturebox
        }


        //Basic Copy - scan all pixels then copy those pixels to another image (blank) then set the picture box 2 to the result image. All the image process this would be the template since you have to traverse to each image
        private void button3_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    resultimage.SetPixel(x, y, pixel);
//                  resultimage.SetPixel(x, y, Color.blue); the entire image would become blue

                }
            }
            pictureBox2.Image = resultimage;
        }

        //Grayscale
        private void button4_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3; //equidistant values - same values 
                    //sometimes averaging is not recommended, percentages are more specific since it gives you distinct gray
                    resultimage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                    //Color.FromArgb - specifies the red,green and blue to specify a new color
                }
            }
            pictureBox2.Image = resultimage;
        }

        //Invert
        private void button5_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    resultimage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                    //assuming everything is light then we add then we subtract darkness
                    //turns into cymk-ish picture
                }
            }
            pictureBox2.Image = resultimage;
        }
        
        //Histogram - image is converted to data to make it more understandable or to do the mathematical scale 
        private void button6_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);

            //Setting the image to gray
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    resultimage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }

            Color sample;
            int[] histdata = new int[256];
            
            //Getting the frequency of intensity levels of the pixels 
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    sample = resultimage.GetPixel(x, y); //0-255 which is r,g,b
                    histdata[sample.R]++; //it doesn't matter if it is .G/.B since they are all gray anyways
                }
            }
            
            //Setting the background image of the picture
            Bitmap mydata = new Bitmap(256, 800); //256 since it is 0-255 for the width and the height can be any size.
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    mydata.SetPixel(x, y, Color.White);
                }
            }

            //plot histdata
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histdata[x] / 5, 800); y++) //some data goes beyond the image width so you can divide them
                {
                    mydata.SetPixel(x, 799 - y, Color.Black); //799 minus to prevent upside down image
                }
            }
            pictureBox2.Image = mydata;
            //bright = right side
            //dark = left side 
        }

        //Save
        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "untitled";
            saveFileDialog1.Filter = " Joint Photographic Experts Group (*.jpg)|*.jpeg|Portable Network Graphics (*.png)|*.png";
            saveFileDialog1.ShowDialog(); //shows the saveFileDialog
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resultimage.Save(saveFileDialog1.FileName); //saving an image
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "Bright")
            {
                callBright(trackBar1.Value);
            }
            if (e.ClickedItem.Text == "Contrast")
            {
                equalization(ref loadimage, ref resultimage, trackBar3.Value); //also have to put ref
                pictureBox2.Image = resultimage; 
            }
            if (e.ClickedItem.Text == "Flip Horiz")
            {
                resultimage = new Bitmap(loadimage.Width, loadimage.Height);
                for (int x = 0; x < loadimage.Width; x++)
                {
                    for (int y = 0; y < loadimage.Height; y++)
                    {
                        Color pixel = loadimage.GetPixel(x, y);
                        resultimage.SetPixel((loadimage.Width - 1) -  x,  y, pixel);
                        //width going to 0 since it is horizontal
                        //-1 since it starts with 0, minus x since width
                    }
                }
                pictureBox2.Image = resultimage;
            }
            if (e.ClickedItem.Text == "Flip Vert")
            {
                resultimage = new Bitmap(loadimage.Width, loadimage.Height);
                for (int x = 0; x < loadimage.Width; x++)
                {
                    for (int y = 0; y < loadimage.Height; y++)
                    {
                        Color pixel = loadimage.GetPixel(x, y);
                        resultimage.SetPixel(x, (loadimage.Height - 1)- y, pixel);
                        //just like the horiz, but from height to 0 
                    }
                }
                pictureBox2.Image = resultimage;
            }

            if(e.ClickedItem.Text == "Colored Contrast")
            {
                equalizationColored(ref loadimage, ref resultimage, trackBar3.Value); //also have to put ref
                pictureBox2.Image = resultimage;
            }
        }

        //Brightness
        public void callBright(int val)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    //Value has changed due to a specific intensity
                    //Val might go over 255 which is why we have to chose 255 or the min or the vice versa but with 0
                    if(val >= 0)
                        resultimage.SetPixel(x, y, Color.FromArgb(Math.Min(pixel.R + val, 255), Math.Min(pixel.G + val, 255), Math.Min(pixel.G + val, 255)));
                    else
                        resultimage.SetPixel(x, y, Color.FromArgb(Math.Max(pixel.R + val, 0), Math.Max(pixel.G + val, 0), Math.Max(pixel.G + val, 0)));

                }
            }
            pictureBox2.Image = resultimage;
        }

        //Constrast have to equalized
        public void equalizationColored(ref Bitmap a, ref Bitmap b, int degree) //a = loaded image; b = resulting image
        {
            int height = a.Height;
            int width = a.Width;
            int numSamples, histSum;
            int[] Ymap = new int[256];
            int[] hist = new int[256];
            int percent = degree;

            int[] YmapR = new int[256];
            int[] YmapG = new int[256];
            int[] YmapB = new int[256];
            int[] histR = new int[256];
            int[] histG = new int[256];
            int[] histB = new int[256];

            // compute the histogram from the sub-image
            Color colorRed;
            Color colorBlue;
            Color colorGreen;

            
            //Compute Red and Add to Histogram 
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    colorRed = a.GetPixel(x, y);
                    histR[colorRed.R]++;

                }
            }

            //Compute Green and Add to Histogram 
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    colorGreen = a.GetPixel(x, y);
                    histG[colorGreen.G]++;

                }
            }

            //Compute Blue and Add to Histogram 
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    colorBlue = a.GetPixel(x, y);
                    histB[colorBlue.B]++;

                }
            }


            // remap the Ys, use the maximum contrast (percent == 100)
            // based on histogram equalization
            numSamples = (a.Width * a.Height); // # of samples that contributed to the histogram
            int histSumR = 0;
            int histSumG = 0;
            int histSumB = 0;

            //Red
            for (int h = 0; h < 256; h++)
            {
                histSumR += histR[h];
                YmapR[h] = histSumR * 255 / numSamples;
            }

            // if desired contrast is not maximum (percent < 100), then adjust the mapping
            if (percent < 100)
            {
                for (int h = 0; h < 256; h++)
                {
                    YmapR[h] = h + ((int)YmapR[h] - h) * percent / 100;

                }
            }

            //Green
            for (int h = 0; h < 256; h++)
            {
                histSumG += histG[h];
                YmapG[h] = histSumG * 255 / numSamples;
            }

            // if desired contrast is not maximum (percent < 100), then adjust the mapping
            if (percent < 100)
            {
                for (int h = 0; h < 256; h++)
                {
                    YmapG[h] = h + ((int)YmapG[h] - h) * percent / 100;
                }
            }

            //Blue
            for (int h = 0; h < 256; h++)
            {
                histSumB += histB[h];
                YmapB[h] = histSumB * 255 / numSamples;
            }

            // if desired contrast is not maximum (percent < 100), then adjust the mapping
            if (percent <  100)
            {
                for (int h = 0; h < 256; h++)
                {
                    YmapB[h] = h + ((int)YmapB[h] - h) * percent / 100;
                  
                }
            }



            b = new Bitmap(a.Width, a.Height);
            // enhance the region by remapping the intensities
            for (int y = 0; y < a.Height; y++)
            {
                for (int x = 0; x < a.Width; x++)
                {
                    // set the new value of the color value
                    if (degree >= 0)
                    {
                        Color temp = Color.FromArgb(Math.Min(YmapR[a.GetPixel(x, y).R], 255), Math.Min(YmapG[a.GetPixel(x, y).G], 255), Math.Min(YmapB[a.GetPixel(x, y).B], 255));
                        b.SetPixel(x, y, temp);
                    }
                    else
                    {
                        Color temp = Color.FromArgb(Math.Max(YmapR[a.GetPixel(x, y).R], 255), Math.Max(YmapG[a.GetPixel(x, y).G], 255), Math.Max(YmapB[a.GetPixel(x, y).B], 255));
                        b.SetPixel(x, y, temp);
                    }
                }
            }
            
        }


        //Contrast - evenly distributing the value or magnitude of data over the histogram 
        //ref is a pointer 
        public void equalization(ref Bitmap a, ref Bitmap b, int degree) //a = loaded image; b = resulting image
        {
            int height = a.Height;
            int width = a.Width;
            int numSamples, histSum;
            int[] Ymap = new int[256];
            int[] hist = new int[256];
            int percent = degree;
            
            // compute the histogram from the sub-image
            Color nakuha;
            Color gray;
            Byte graydata;

            //compute greyscale 
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    nakuha = a.GetPixel(x, y);
                    graydata = (byte)((nakuha.R + nakuha.G + nakuha.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(x, y, gray);
                }
            }
            
            //histogram 1d data;
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    nakuha = a.GetPixel(x, y);
                    hist[nakuha.B]++; //took the histogram date for the 1d data

                }
            }
            
            // remap the Ys, use the maximum contrast (percent == 100)
            // based on histogram equalization
            numSamples = (a.Width * a.Height); // # of samples that contributed to the histogram
            histSum = 0;
            for (int h = 0; h < 256; h++)
            {
                histSum += hist[h];
                Ymap[h] = histSum * 255 / numSamples; //if the data is greater then distribute them the left or the right
            }

            // if desired contrast is not maximum (percent < 100), then adjust the mapping
            if (percent < 100)
            {
                for (int h = 0; h < 256; h++)
                {
                    Ymap[h] = h + ((int)Ymap[h] - h) * percent / 100;
                }
            }

            b = new Bitmap(a.Width, a.Height);
            // enhance the region by remapping the intensities
            for (int y = 0; y < a.Height; y++)
            {
                for (int x = 0; x < a.Width; x++)
                {
                    // set the new value of the gray value
                    Color temp = Color.FromArgb(Ymap[a.GetPixel(x, y).R], Ymap[a.GetPixel(x, y).G], Ymap[a.GetPixel(x, y).B]);
                    b.SetPixel(x, y, temp);
                }
            }
        }
         
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        //Rotate
        private void button8_Click(object sender, EventArgs e)
        {
            Rotate(ref loadimage, ref resultimage, trackBar2.Value);
            pictureBox2.Image = resultimage;
        }

        //Rotation - according to the z axis 
        public static void Rotate(ref Bitmap a, ref Bitmap b, int value)
        {
            //Concert Angle to Radians
            float angleRadians = (float)(value * Math.PI / 180); //compute according to radians or to degrees (?)
            //Translation: changing the axis points of the rotation since the axis point rotation starts at the center
            //Move the center to the origin since it might rotate it respect to the origin

            //Get the center of the image
            int xCenter = (int)(a.Width / 2);
            int yCenter = (int)(a.Height / 2);
            int width, height, xs, ys, xp, yp, x0, y0;
            float cosA, sinA;

            //Get the cos and sine of the Radians
            cosA = (float)Math.Cos(angleRadians);
            sinA = (float)Math.Sin(angleRadians);

            //Assign a width and height
            width = a.Width;
            height = a.Height;
            b = new Bitmap(width, height);

            //Nested Loop
            for (xp = 0; xp < width; xp++)
            {
                for (yp = 0; yp < height; yp++)
                {
                    //Move to the 0,0 
                    x0 = xp - xCenter; // translate to (0,0)
                    y0 = yp - yCenter;

                    //Rotate pixel
                    xs = (int)(x0 * cosA + y0 * sinA); // rotate around the origin
                    ys = (int)(-x0 * sinA + y0 * cosA);
                    xs = (int)(xs + xCenter); // translate back to (xCenter,yCenter)l go back to the center
                    ys = (int)(ys + yCenter);

                    //Restrictions
                    xs = Math.Max(0, Math.Min(width - 1, xs)); // force the source location to within image bounds it might go outside the bonds 
                    ys = Math.Max(0, Math.Min(height - 1, ys));

                    //Set the pixels
                    b.SetPixel(xp, yp, a.GetPixel(xs, ys));
                }
            }
        }

        //Scale
        private void button9_Click(object sender, EventArgs e)
        {
            Scale(ref loadimage, ref resultimage, 50, 50 ); //target width and target height
            pictureBox3.Image = resultimage;
        }

        //Resizing or Lossy Scale/Expansion
        //Large to Small - would lose pixel
        //Small to Large - duplicate pixels
        public static void Scale(ref Bitmap a, ref Bitmap b, int nwidth, int nheight)
        {
            int targetWidth = nwidth;
            int targetHeight = nheight;
            int xTarget, yTarget, xSource, ySource;
            int width = a.Width;
            int height = a.Height;
            b = new Bitmap(targetWidth, targetHeight);

            for (xTarget = 0; xTarget < targetWidth; xTarget++)
            {
                for (yTarget = 0; yTarget < targetHeight; yTarget++)
                {
                    xSource = xTarget * width / targetWidth;
                    ySource = yTarget * height / targetHeight;
                    b.SetPixel(xTarget, yTarget, a.GetPixel(xSource, ySource));
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        //When the form is loaded then the code gets executed
        private void Form1_Load(object sender, EventArgs e)
        {
            myDevices = DeviceManager.GetAllDevices();  //gets all devices and put them in an array 
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            myDevices[0].ShowWindow(pictureBox1); //Display the Camera
        }

        private void button11_Click(object sender, EventArgs e)
        {
            myDevices[0].Stop(); //Disconnect to the camera 
        }

        private void button12_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true; 
            //timer is a code that runs according to interval 
        }

        private void timer1_Tick(object sender, EventArgs e) //to make it as an actual moving object timer is used. 
        {
            IDataObject data; //implicit data which is polymorphic that accepts any kind of data
            Image bmap; //simply an image
            myDevices[0].Sendmessage(); //print screen
            data = Clipboard.GetDataObject(); //printscreen of the camera preview 
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true)); //convert that to an image 
            Bitmap b = new Bitmap(bmap); //taking a frame fro, tje ca,era 
            resultimage = new Bitmap(b.Width, b.Height);
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color pixel = b.GetPixel(x, y); //a lot of overhead functions 
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    resultimage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }
            pictureBox2.Image = resultimage;
            //this would have a lag because of the computations, 
        }

        private void button13_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false; 
        }

        private void button16_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false; 
        }

        private void button15_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            myDevices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);
            BitmapFilter.GrayScale(b);
            //BitmapFilter.Invert(b); //if there is an error, the overhead is too high because of the interval 
            //BitmapFilter.TimeWarp(b, 10, false);
            //BitmapFilter.Sphere(b,false);
            //BitmapFilter.EmbossLaplacian(b);
            //BitmapFilter.Smooth(b, 50);
            pictureBox2.Image = b; //turns the image to a bitmap that is already filtered 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //Sepia
        private void button7_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    int redS = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int greenS = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int blueS = (int)(0.272 * r + 0.534 * g + 0.131 * b);


                    if (redS > 255)
                    {
                        r = 255;
                    }
                    else
                    {
                        r = redS;
                    }

                    if (greenS > 255)
                    {
                        g = 255;
                    }
                    else
                    {
                        g = greenS;
                    }

                    if (blueS > 255)
                    {
                        b = 255;
                    }
                    else
                    {
                        b = blueS;
                    }


                    resultimage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            pictureBox2.Image = resultimage;
        }
    }
}
