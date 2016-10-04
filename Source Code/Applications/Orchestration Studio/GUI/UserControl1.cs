using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orchestration_Studio.GUI
{
   

    public partial class UserControl1 : UserControl
    {
        public int state = 1;

        public UserControl1(string app,string name,int gaverage,int minaverage, int maxaverage,int profiling)
        {
            InitializeComponent();
            textBox1.Text = app;
            textBox2.Text = name;
            textBox3.Text = gaverage.ToString();
            textBox4.Text = minaverage.ToString();
            textBox5.Text = maxaverage.ToString();
            textBox6.Text = profiling.ToString();
            label10.Text = "NONE :";
            label11.Text = "NONE :";
            label12.Text = "NONE :";
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;

            if (textBox1.Text == "Matrix Multiplication")
            {
                label10.Text = "Jobs :";
                label11.Text = "Matrices Size :";
                label12.Text = "Kernels :";
                comboBox2.Enabled = true;
                comboBox2.Text = "1000";
                comboBox2.Items.Add("1000");
                comboBox2.Items.Add("1250");
                comboBox2.Items.Add("1500");
                comboBox2.Items.Add("1750");
                comboBox2.Items.Add("2000");
                comboBox2.SelectedIndex = 0;
                comboBox3.Enabled = true;
                comboBox3.Text = "20";
                comboBox3.Items.Add("20");
                comboBox3.Items.Add("30");
                comboBox3.Items.Add("40");
                comboBox3.Items.Add("50");
                comboBox3.Items.Add("60");
                comboBox3.Items.Add("70");
                comboBox3.Items.Add("80");
                comboBox3.Items.Add("90");
                comboBox3.Items.Add("100");
                comboBox3.SelectedIndex = 0;
                comboBox4.Enabled = true;
                comboBox4.Text = "2CPU";
                comboBox4.Items.Add("2CPU 2GPPU");
                comboBox4.Items.Add("2CPU 2noGPPU");
                comboBox4.Items.Add("2CPU 2noGPPU 2GPPU");
                comboBox4.SelectedIndex = 0;
            }


            if (textBox1.Text == "Numbers Addition")
            {
                label10.Text = "Type :";
                comboBox2.Enabled = true;
                comboBox2.Text = "Orchestrated";
                comboBox2.Items.Add("Orchestrated");
                comboBox2.Items.Add("non-Orchestrated");

            }


            if (textBox1.Text == "Workload Maker")
            {
                label3.Enabled = false;
                label4.Enabled = false;
                label6.Enabled = false;
                label7.Enabled = false;
                label8.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox9.Text = "-t 70 -i 10";
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox6.Enabled = false;
                label5.Text = "Threads/Time(sec) :";
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                comboBox1.Enabled = false;
            }


            if(textBox1.Text== "Image Processing Sobel")
            {
                label10.Text = "Image :";
                label11.Text = "Passes :";
                label12.Text = "Kernels :";


                comboBox2.Enabled = true;
                comboBox2.Text = "HD - 1280x720";
                comboBox2.Items.Add("XS - 75x75");
                comboBox2.Items.Add("XM - 200x200");
                comboBox2.Items.Add("CM - 500x500");
                comboBox2.Items.Add("HD - 1280x720");
                comboBox2.SelectedItem = 3;
           

                comboBox3.Enabled = true;
                comboBox3.Text = "1";
                comboBox3.Items.Add("1");
                comboBox3.Items.Add("2");
                comboBox3.Items.Add("5");
                comboBox3.Items.Add("10");
                comboBox3.Items.Add("50");
                comboBox3.Items.Add("100");
                comboBox3.Items.Add("500");
                comboBox3.SelectedItem = 0;

                comboBox4.Enabled = true;
                comboBox4.Text = "2CPU";
                comboBox4.Items.Add("2CPU");
                comboBox4.Items.Add("2CPU 2PCIe");
                comboBox4.Items.Add("2CPU 2GPPU");
                comboBox4.Items.Add("2CPU 2noGPPU");
                comboBox4.Items.Add("2CPU 2noGPPU 2GPPU");
                comboBox4.Items.Add("2CPU 2PCIe 2GPPU");
                comboBox4.Items.Add("2CPU 2PCIe 2noGPPU");
                comboBox4.Items.Add("2CPU 2PCIe 2GPPU 2noGPPU");
                comboBox4.SelectedItem = 0;
            }
            

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Execute")
            {
                textBox2.ReadOnly = true;
                button3.Enabled = false;
                textBox6.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                button2.Text = "Abort";
                if (textBox1.Text == "Workload Maker")
                {
                    button2.Enabled = false;
                    button3.Enabled = true;
                }
                Classes.AppExecution.Execute(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text,  textBox7.Text, comboBox1.SelectedItem.ToString(), textBox8.Text, textBox9.Text,comboBox2.Text, comboBox3.Text.ToString(), comboBox4.Text.ToString());
            }
            else
            {
                Program.watcher.sendCommands.Add("stop:" + textBox2.Text);
                this.Dispose();           
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
         }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.watcher.sendCommands.Add("setipolicy:" + textBox2.Text + ":" + comboBox1.SelectedItem.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.watcher.sendCommands.Add("setms:" + textBox2.Text + ":" + textBox3.Text + ":" + textBox4.Text + ":" + textBox5.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.watcher.sendCommands.Add("setpriority:" + textBox2.Text + ":" + textBox7.Text);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            if(state==1)
            {
                state = 0;

                Size = new Size(264, 34);
            }
            else
            {
                state = 1;
                Size = new Size(264, 352);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "Numbers Addition")
            {
                if (comboBox2.SelectedItem.ToString() == "non-Orchestrated")
                {
                    label3.Enabled = false;
                    label4.Enabled = false;
                    label6.Enabled = false;
                    label7.Enabled = false;
                    label8.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    textBox6.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                    comboBox1.Enabled = false;
                }
                else
                {
                    label3.Enabled = true;
                    label4.Enabled = true;
                    label6.Enabled = true;
                    label7.Enabled = true;
                    label8.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;

                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    textBox6.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    comboBox1.Enabled = true;
                }
            }
        }
    }
}
