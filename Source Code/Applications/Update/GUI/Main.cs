using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update.GUI
{
    delegate void dChangeStatus(string text);
    public partial class Main : Form
    {
        private dynamic app_info;
        private dynamic app_actions;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load_1(object sender, EventArgs e)
        {
            try
            {
                app_info = Activator.CreateInstance(Program.info_class);
                app_actions = Activator.CreateInstance(Program.actions_class);
                label2.Text = app_info.getUpperTitle();
                label3.Text = app_info.getBottomTitle();
                label4.Text = app_info.getTitle(); 
                this.pictureBox2.Image = XResources.spinner;
            }
            catch(Exception ex)
            {
                (new GUI.Error(ex)).ShowDialog();
                Environment.Exit(-1);
            }
            backgroundWorker1.RunWorkerAsync();
        }

        public void ChangeStatus(string text)
        {
            if (this.label1.InvokeRequired)
            {
                dChangeStatus d = new dChangeStatus(ChangeStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                label1.Text = text;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                app_actions.Execute(this);
                ProcessStartInfo info = new ProcessStartInfo(@"" + Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), app_info.getExecutable()));
                info.UseShellExecute = true;
                info.Arguments = "\"updated\"";
                try
                {
                    Process.Start(info);
                }
                catch (Win32Exception ex)
                {
                }
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                (new GUI.Error(ex)).ShowDialog();
                Environment.Exit(-1);
            }
        }

        public void Elevate()
        {
            Program.Elevate();
        }
    }
}
