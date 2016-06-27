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
    public partial class EncryptionPassword : Form
    {
        public EncryptionPassword()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.password = textBox1.Text;
            this.Dispose();
        }
    }
}
