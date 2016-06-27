using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update.GUI
{
    public partial class Error : Form
    {
        private Exception ex;
        public Error(Exception ex)
        {
            InitializeComponent();
            this.ex = ex;
        }

        private void Error_Load(object sender, EventArgs e)
        {
            label1.Text = ex.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
