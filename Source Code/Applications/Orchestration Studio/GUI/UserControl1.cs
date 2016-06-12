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
        public UserControl1(string app,string name,int gaverage,int minaverage, int maxaverage,int profiling)
        {
            InitializeComponent();
            textBox1.Text = app;
            textBox2.Text = name;
            textBox3.Text = gaverage.ToString();
            textBox4.Text = minaverage.ToString();
            textBox5.Text = maxaverage.ToString();
            textBox6.Text = profiling.ToString();
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
                Classes.AppExecution.Execute(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text,  textBox7.Text, comboBox1.SelectedItem.ToString(), textBox8.Text, textBox9.Text);
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
    }
}
