using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace Orchestration_Studio.GUI
{
    public partial class ConnectionsManager : Form
    {  
        public ConnectionsManager()
        {
            InitializeComponent();
        }

        private void ConnectionsManager_Load(object sender, EventArgs e)
        {

            if(!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"OrchestrationST")))
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OrchestrationST"));
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OrchestrationST", "connections.isca")))
                File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OrchestrationST", "connections.isca"));
            try
            {
                string readText = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OrchestrationST", "connections.isca"));
                Program.connections = JsonConvert.DeserializeObject<List<Classes.Connection>>(readText);
            }
            catch (Exception) { }

            if (Program.connections == null)
                Program.connections = new List<Classes.Connection>();
            ReloadListBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int newconnections = 0;
            for(int i=0;i<listBox1.Items.Count;i++)
            {
                if (listBox1.Items[i].ToString().Contains("New Connection"))
                    newconnections++;
            }
            Program.connections.Add(new Classes.Connection("New Connection " + newconnections,"","","1102"));
            ReloadListBox();
        }


        public void ReloadListBox()
        {
            listBox1.Items.Clear();
            foreach(Classes.Connection connection in Program.connections)
                listBox1.Items.Add(connection.name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                textBox1.Text = Program.connections[listBox1.SelectedIndex].name;
                textBox2.Text = Program.connections[listBox1.SelectedIndex].host;
                textBox4.Text = Program.connections[listBox1.SelectedIndex].username;
                textBox5.Text = Program.connections[listBox1.SelectedIndex].port;
                button4.Text = "Edit";
                button4.Enabled = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;

                button6.Visible = false;
                button3.Enabled = true;
            }catch(Exception)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Edit")
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    button4.Text = "Save";
                    button6.Visible = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                }
            }
            else if(button4.Text=="Save")
            {
                int selected = listBox1.SelectedIndex;
                Program.connections[listBox1.SelectedIndex].name =  textBox1.Text;
                Program.connections[listBox1.SelectedIndex].host = textBox2.Text;
                Program.connections[listBox1.SelectedIndex].username = textBox4.Text;
                Program.connections[listBox1.SelectedIndex].port = textBox5.Text;
                ReloadListBox();
                listBox1.SelectedIndex = selected;
            }
        }

        private void ConnectionsManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Program.connections.Count != 0)
                {
                    string result = JsonConvert.SerializeObject(Program.connections);

                    using (FileStream writter = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OrchestrationST", "connections.isca")))
                        writter.Write(Encoding.ASCII.GetBytes(result), 0, Encoding.ASCII.GetBytes(result).Length);
                }
            }catch(Exception)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.connections.RemoveAt(listBox1.SelectedIndex);
            ReloadListBox();
            button4.Text = "Edit";
            button4.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            button6.Visible = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            (new GUI.EncryptionPassword()).ShowDialog();

            Program.client = new Renci.SshNet.SshClient(Program.connections[listBox1.SelectedIndex].host, Program.connections[listBox1.SelectedIndex].username, Program.password);

            Program.client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 5);
            try
            {
                Program.client.Connect();
            }
            catch (Exception) { }
            if (Program.client.IsConnected != true)
            {
                MessageBox.Show("Connection Failed", "There was an error connecting to " + Program.connections[listBox1.SelectedIndex].host + ". Please try again.");
                Program.main.toolStripStatusLabel4.Text = "Disconnected";
                Program.main.toolStripStatusLabel4.ForeColor = Color.Red;
                Program.selectedConnection = -1;
            }
            else
            {
                IDictionary<Renci.SshNet.Common.TerminalModes, uint> termkvp = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
                termkvp.Add(Renci.SshNet.Common.TerminalModes.ECHO, 53);
                Program.shellStream = Program.client.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);
                Program.shellStream.Flush();
                Program.main.toolStripStatusLabel4.Text = "Connected";
                Program.main.toolStripStatusLabel4.ForeColor = Color.Green;
                Program.selectedConnection = listBox1.SelectedIndex;
                this.Hide();
            }
        }
    }
}
