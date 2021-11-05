using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DipCS
{
    public partial class Form1 : Form
    {

        Bitmap loadimage, resultimage;

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadimage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadimage;

        }


        private void button3_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height); 
            for(int x=0; x<loadimage.Width; x++)
            {
                for(int y=0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    resultimage.SetPixel(x, y, pixel); 
                }
            }
            pictureBox2.Image = resultimage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    resultimage.SetPixel(x, y, Color.FromArgb(grey,grey,grey));
                }
            }
            pictureBox2.Image = resultimage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    Color pixel = loadimage.GetPixel(x, y);
                    resultimage.SetPixel(x, y, Color.FromArgb(255-pixel.R, 255-pixel.G, 255-pixel.B));
                }
            }
            pictureBox2.Image = resultimage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            resultimage = new Bitmap(loadimage.Width, loadimage.Height);

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
            for (int x = 0; x < loadimage.Width; x++)
            {
                for (int y = 0; y < loadimage.Height; y++)
                {
                    sample = resultimage.GetPixel(x, y);
                    histdata[sample.R]++; 
                }
            }
            Bitmap mydata = new Bitmap(256, 800);
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
                for (int y = 0; y < Math.Min(histdata[x]/5,800); y++)
                {
                    mydata.SetPixel(x, 799-y, Color.Black);
                }
            }
            pictureBox2.Image = mydata;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resultimage.Save(saveFileDialog1.FileName); 
        }

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


                    if(redS > 255)
                    {
                        r = 255; 
                    } else
                    {
                        r = redS;
                    }

                    if(greenS > 255)
                    {
                        g = 255; 
                    } else
                    {
                        g = greenS; 
                    } 

                    if(blueS > 255)
                    {
                        b = 255; 
                    } else
                    {
                        b = blueS; 
                    }


                    resultimage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            pictureBox2.Image = resultimage;
        }

        public Form1()
        {
            InitializeComponent();
        }

    }
}
