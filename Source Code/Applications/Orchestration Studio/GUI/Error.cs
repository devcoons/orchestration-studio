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
    public partial class Error : Form
    {
        public Error()
        {
            InitializeComponent();
        }

        public Error(string arg)
        {
            InitializeComponent();
            label1.Text = arg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Error_Load(object sender, EventArgs e)
        {

        }
    }
}
