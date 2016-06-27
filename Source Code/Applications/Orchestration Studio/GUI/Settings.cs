using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orchestration_Studio.GUI
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            comboBox1.Items.Add("- None -");
            comboBox1.Items.Add("PowerSaving (amps)");
            comboBox1.Items.Add("PowerSaving (watts)");


        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Program.watcher.statsRefreshRate.ToString();
            textBox2.Text = Program.watcher.loopRefreshRate.ToString();

            comboBox1.SelectedItem = Program.watcher.globalPolicy;
            if(Program.watcher.globalPolicy == "- None -")
            {
                textBox3.Text = "";
                textBox3.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                textBox2.Text = Program.watcher.globalPolicyParameters;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.watcher.statsRefreshRate = int.Parse(textBox1.Text);
            Program.watcher.loopRefreshRate = int.Parse(textBox2.Text);
            Program.watcher.globalPolicy = comboBox1.SelectedItem.ToString();
            Program.watcher.globalPolicyParameters = textBox3.Text;


            this.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "- None -")
            {
                textBox3.Text = "";
                textBox3.Enabled = false;
            }
            if (comboBox1.SelectedItem.ToString() == "PowerSaving (amps)")
            {
                textBox3.Text = "1";
                textBox3.Enabled = true;
            }

            if (comboBox1.SelectedItem.ToString() == "PowerSaving (watts)")
            {
                textBox3.Text = "25";
                textBox3.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
