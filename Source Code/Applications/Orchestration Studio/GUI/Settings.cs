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
            comboBox1.Items.Add("- Free -");
            comboBox1.Items.Add("Power Kl.Agv Restrictor");
            comboBox1.Items.Add("Current Kl.Agv Restrictor");
            comboBox1.Items.Add("Utilization Kl.Agv Restrictor");
            if (Program.watcher.globalPolicy == "- Free -")
                comboBox1.SelectedIndex = 0;
            if (Program.watcher.globalPolicy == "Power Kl.Agv Restrictor")
                comboBox1.SelectedIndex = 1;
            if (Program.watcher.globalPolicy == "Current Kl.Agv Restrictor")
                comboBox1.SelectedIndex = 2;
            if (Program.watcher.globalPolicy == "Utilization Kl.Agv Restrictor")
                comboBox1.SelectedIndex = 3;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = Program.watcher.statsRefreshRate.ToString();
            textBox2.Text = Program.watcher.loopRefreshRate.ToString();

            comboBox1.SelectedItem = Program.watcher.globalPolicy;
            if(Program.watcher.globalPolicy == "- Free -")
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
    //        Program.watcher.loopRefreshRate = int.Parse(textBox2.Text);
            Program.watcher.globalPolicy = comboBox1.SelectedItem.ToString();
            Program.watcher.globalPolicyParameters = textBox3.Text;
            if (Program.watcher.globalPolicy == "- Free -")
                Program.watcher.sendCommands.Add("setgpolicy:free:" + Program.watcher.globalPolicyParameters);
            if (Program.watcher.globalPolicy == "Power Kl.Agv Restrictor")
                Program.watcher.sendCommands.Add("setgpolicy:PowerBalancing:" + Program.watcher.globalPolicyParameters);
            if (Program.watcher.globalPolicy == "Current Kl.Agv Restrictor")
                Program.watcher.sendCommands.Add("setgpolicy:CurrentBalancing:" + Program.watcher.globalPolicyParameters);
            if (Program.watcher.globalPolicy == "Utilization Kl.Agv Restrictor")
                Program.watcher.sendCommands.Add("setgpolicy:UtilizationBalancing:" + Program.watcher.globalPolicyParameters);
      
                Program.main.toolStripStatusLabel7.Text = " [" + Program.watcher.globalPolicyParameters + "]" + Program.watcher.globalPolicy;
          
            this.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "- Free -")
            {
                textBox3.Text = "";
                textBox3.Enabled = false;
            }
            if (comboBox1.SelectedItem.ToString() == "Power Kl.Agv Restrictor")
            {
                textBox3.Text = "4000";
                textBox3.Enabled = true;
            }
            if (comboBox1.SelectedItem.ToString() == "Current Kl.Agv Restrictor")
            {
                textBox3.Text = "600";
                textBox3.Enabled = true;
            }
            if (comboBox1.SelectedItem.ToString() == "Utilization Kl.Agv Restrictor")
            {
                textBox3.Text = "45";
                textBox3.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
