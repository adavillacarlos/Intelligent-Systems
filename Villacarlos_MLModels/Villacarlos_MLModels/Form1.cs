using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Backprop; 

namespace Villacarlos_MLModels
{
    public partial class Form1 : Form
    {
        NeuralNet nn; 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            nn = new NeuralNet(2,5, 1); 
            // x (input) - meaning it can put x numbers of input which are interconnected to the neural compute neurons; 1 input is connected to each neurons,
            // neural compute neurons - it has n neural neuros which is also connected to the output, if you add more neuronet, it would have more time complexity
            // y(output) -- y number of output  
            // neural compute neurons  == braincells or brain neurons in terms of the brain, the more you have it would memorize more. 

            //Machine Learning - 3 layers only 
            //Deep Learning - if there is >3 layers; eg. CNN since it consist 15 - n layers. 


        }

        //1st thing you do after you created a model. 
        //In this phase, you tell the neuronet what inputs do you want and what is the corresponding output that you want to receive. 
        //e.g. if you want to input 0,0, your ouput would be 0 for an AND operation. 

        //This is an example of supervised classification model since you are teaching the model what appropriate inputs should mertis a correct output for the AND logic operation. 

        //Overfit - training the model a lot like with the 0 AND 0 
        //Underfit - you train the model less like having a less epoch or less neuronet  

        private void button2_Click(object sender, EventArgs e)
        {

            for (int c = 0; c <500; c++)
            {
                // 1 AND 1 - 1 training model 
                nn.setInputs(0, 1.0); //index 0 means the position of the input
                nn.setInputs(1, 1.0); //index 1 position so if there is a third one, it would be nn.setInputs(2,input); 
                nn.setDesiredOutput(0, 1.0); //only 1 ouput so only index 0 
                nn.learn(); //understand what your set items, memorize and understand. 

                // 0 AND 1 
                nn.setInputs(0, 0.0);
                nn.setInputs(1, 1.0);
                nn.setDesiredOutput(0, 0.0);
                nn.learn();

                //1 AND 0 
                nn.setInputs(0, 1.0);
                nn.setInputs(1, 0.0);
                nn.setDesiredOutput(0, 0.0);
                nn.learn();

                //0 AND 0 
                nn.setInputs(0, 0.0);
                nn.setInputs(1, 0.0);
                nn.setDesiredOutput(0, 0.0);
                nn.learn();


                //this is considered as one iteration of training or 1 epoch since the loop is set to 500 it means it has 500 epochs 
            }
            //After training you can now put inputs and test it. 
        }

        //You need to find the right size model for the system and consider the times how many times you would train the system 
        private void button3_Click(object sender, EventArgs e)
        {
            nn.setInputs(0, Convert.ToDouble(textBox1.Text));
            nn.setInputs(1, Convert.ToDouble(textBox2.Text));
            nn.run();
            textBox3.Text = "" + nn.getOuputData(0);
        }
        //If the output displays >=0.5 it is represented as True or logic 1 but if it is <0.5 it is represented as False or logic 0. 


        private void button4_Click(object sender, EventArgs e)
        {
            for (int c = 0; c < 1000; c++)
            {
                // 1 AND 1 
                nn.setInputs(0, 1.0);
                nn.setInputs(1, 1.0);
                nn.setDesiredOutput(0, 1.0);
                nn.learn();

                // 0 AND 1 
                nn.setInputs(0, 0.0);
                nn.setInputs(1, 1.0);
                nn.setDesiredOutput(0, 1.0);
                nn.learn();

                //1 AND 0 
                nn.setInputs(0, 1.0);
                nn.setInputs(1, 0.0);
                nn.setDesiredOutput(0, 1.0);
                nn.learn();

                //0 AND 0 
                nn.setInputs(0, 0.0);
                nn.setInputs(1, 0.0);
                nn.setDesiredOutput(0, 0.0);
                nn.learn();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            nn.setInputs(0, Convert.ToDouble(textBox1.Text));
            nn.setInputs(1, Convert.ToDouble(textBox2.Text));
            nn.run();
            textBox3.Text = "" + nn.getOuputData(0);
        }
    }
}
