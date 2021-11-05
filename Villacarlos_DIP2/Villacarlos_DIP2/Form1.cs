using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebCamLib;
using ImageProcess2;


namespace Villacarlos_DIP2
{
    public partial class Form1 : Form
    {

        Bitmap imageB, imageA, colorgreen;
        Bitmap resultImage;
        Device[] myDevices; 

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = imageB;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog(); 
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog2.FileName);
            pictureBox2.Image = imageA; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(imageB.Width, imageB.Height);
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5; 
            for(int x=0; x<imageB.Width; x++)
            {
                for(int y=0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int substractvalue = Math.Abs(grey - greygreen);
                    if (substractvalue > threshold)
                    {
                        resultImage.SetPixel(x, y, pixel);
                    } else
                    {
                        resultImage.SetPixel(x, y, backpixel);

                    }
                }
            }
            pictureBox3.Image = resultImage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myDevices = DeviceManager.GetAllDevices();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myDevices[0].ShowWindow(pictureBox1); //Display the Camera

        }

        private void button5_Click(object sender, EventArgs e)
        {
            myDevices[0].Stop(); //Disconnect to the camera 

        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            myDevices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);
            b = ResizeBitmap(b, imageA.Width, imageA.Height); 
            resultImage = new Bitmap(imageA.Width, imageA.Height);


            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    Color pixel = b.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int substractvalue = Math.Abs(grey - greygreen);
                    if (substractvalue > threshold)
                    {
                        resultImage.SetPixel(x, y, pixel);
                    } else
                    {
                        resultImage.SetPixel(x, y, backpixel);

                    }
                }
            }
            pictureBox3.Image = resultImage;
        }

        private Bitmap ResizeBitmap(Bitmap b, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(b, 0, 0, width, height);
            }

            return result;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(); 
        }
    }
}
