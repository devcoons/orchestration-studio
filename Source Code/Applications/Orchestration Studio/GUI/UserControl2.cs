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

        public void UpdateValue(string a)
        {
            label2.Text = a;
        }


    }
}
