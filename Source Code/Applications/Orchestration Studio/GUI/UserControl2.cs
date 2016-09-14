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
    delegate void SetStatsValue(string value);
    public partial class UserControl2 : UserControl
    {
        public UserControl2(string title,string value)
        {
            InitializeComponent();
            label1.Text = title;
            label2.Text = value;
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {

        }

        public string getLabel()
        {
            return label1.Text;
        }


        public void UpdateValue(string value)
        {
            if (this.label2.InvokeRequired)
            {
                SetStatsValue d = new SetStatsValue(UpdateValue);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                label2.Text = value;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
