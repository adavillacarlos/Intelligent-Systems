using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VillacarlosActivity1
{
    public partial class Form1 : Form
    {
        Graphics g;
        int x = 10;
        int y = 10;
        int heightR = 2;
        int heightR2 = 200;
        int flag = 0;
        int flag2 = 0;
        int flag3 = 0;
        int flag4 = 0; 

        int s1 = 50;
        int s2 = 20;
   
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Create solid brush.
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
            SolidBrush greenBrush = new SolidBrush(Color.Green);
            g = e.Graphics;
            g.FillRectangle(greenBrush, s1, s2, 250, heightR2);
            g.FillEllipse(redBrush,100,50,x,y);
            g.FillRectangle(yellowBrush, 110, 130, 120, heightR);
            g.FillEllipse(redBrush, 200, 50, x, y);
         


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /* EYES */ 
            x = x + 3;
            y = y + 3;
            if (x > 50)
            {
                x -= 10;
            }

            if (y > 50)
            {
                y -= 10;
            }

            /* MOUTH */ 
            if (flag==0)
            {
                if (heightR < 50)
                {
                    heightR += 5;
                }
                
                flag = 0;
            }

            if (heightR > 50)
                flag = 1;

            if (flag==1)
            {
                heightR -= 2;
                if (heightR == 2)
                    flag = 0;
            }


            /* HEAD */
        if(flag4 == 0)
            {
             if(flag2 == 0)
                       {
                            s1 += 10;
                            if (s1 == 100)
                            {
                                flag2 = 1; 
                            }   
                        }

                      if(flag2 == 1)
                        {
                            s1 -= 10; 
                            if(s1 == 40)
                            {
                                flag2 = 0;
                                flag4 = 1;
                            }
                        }
            }
         

           if(flag4 == 1)
            {
                if(flag3 == 0)
                {
                    if (heightR2 < 250)
                    {
                        heightR2 += 10;
                    }
                      
                    flag3 = 0; 
                }

                if(heightR2 == 250)
                {
                    flag3 = 1; 
                }

                if (flag3 == 1)
                {
                    heightR2 -= 10; 
                    if(heightR2 < 200)
                    {
                        flag3 = 0;
                        flag4 = 0; 
                    }
                }

            }




            /* 
             * 
             * Rectangle : 50, 20, 250, 200
             * Line: 200, 150, 150, 150
             * g.DrawRectangle(Pens.White, 50, 20, 250, 200);
             * g.DrawLine(Pens.White, 200, 150, 150, 150);
             */

            this.Refresh();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            timer1.Enabled = false; 
        }
    }
}
