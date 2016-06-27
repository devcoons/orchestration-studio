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
        public UserControl1(string app,string name,int throughput)
        {
            InitializeComponent();
            textBox1.Text = app;
            textBox2.Text = name;
            textBox3.Text = throughput.ToString();
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
                button2.Text = "Abort";
                Program.main.AppExecute(textBox1.Text, textBox2.Text, textBox3.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.main.ChangeThroughput(textBox2.Text, textBox3.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
