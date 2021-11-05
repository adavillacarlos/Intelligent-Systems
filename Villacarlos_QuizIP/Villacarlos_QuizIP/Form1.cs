using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Villacarlos_QuizIP
{
    public partial class Form1 : Form
    {

        Bitmap myImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(); 
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            myImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = myImage;
        }


        private void button2_Click(object sender, EventArgs e)
        {

            Bitmap resultImage = new Bitmap(myImage.Width, myImage.Height);



            //Invert  

            for (int x = 0; x < myImage.Width / 2; x++)
            {

                for (int y = 0; y < myImage.Height / 2; y++)
                {

                    Color pixel = myImage.GetPixel(x, y);

                    resultImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));

                }

            }



            //Gray 

            for (int x = myImage.Width / 2; x < myImage.Width; x++)
            {

                for (int y = 0; y < myImage.Height / 2; y++)
                {

                    Color pixel = myImage.GetPixel(x, y);

                    int grey = (pixel.R + pixel.G + pixel.B) / 3;

                    resultImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));

                }

            }



            //Basic Copy  

            for (int x = 0; x < myImage.Width / 2; x++)
            {

                for (int y = myImage.Height / 2; y < myImage.Height; y++)
                {

                    Color pixel = myImage.GetPixel(x, y);

                    resultImage.SetPixel(x, y, pixel);

                }

            }



            //Vertical 

            for (int x = myImage.Width / 2; x < myImage.Width; x++)
            {

                for (int y = myImage.Height / 2; y < myImage.Height; y++)
                {

                    Color pixel = myImage.GetPixel(x, y);

                    resultImage.SetPixel(x, (myImage.Height - 1) - y + myImage.Height / 2, pixel);

                }

            }

            pictureBox1.Image = resultImage;



        }
    }
}

