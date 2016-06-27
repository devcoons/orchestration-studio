using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Orchestration_Studio.GUI
{
    delegate void SetChartApp(List<Classes.AppData> appstats);
    delegate void SetChartSys(List<Classes.SysData> sysstats);
    delegate void SetChartCPU(List<Classes.CPUData> cpustats);
    
    public partial class Main : Form
    {
        public static int selectedView = 0;
        List<GUI.UserControl1> registeredApplications;
        List<GUI.UserControl2> registeredStatistics;
        public Main()
        {
           registeredApplications = new List<UserControl1>();
            registeredStatistics = new List<UserControl2>();
            registeredStatistics.Add(new UserControl2("CPU A59 Average Usage:", "0%"));
            registeredStatistics.Add(new UserControl2("CPU A52 Average Usage:", "0%"));
            registeredStatistics.Add(new UserControl2("CPU A59 Average Power:", "0uW"));
            registeredStatistics.Add(new UserControl2("CPU A52 Average Power:", "0uW"));
            registeredStatistics.Add(new UserControl2("CPU A59 Average uA:", "0uA"));
            registeredStatistics.Add(new UserControl2("CPU A52 Average uA:", "0uA"));
            registeredStatistics.Add(new UserControl2("SYS Board Average Power:", "0uW"));
            registeredStatistics.Add(new UserControl2("SYS Board Average uA:", "0uA"));
            InitializeComponent();
            splitContainer3.SplitterDistance = splitContainer3.Width - 255;
            panel1.Hide();
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 2;
            chart1.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart1.ChartAreas["ChartArea1"].AxisX.InterlacedColor = Color.Silver;
            chart1.ChartAreas["ChartArea1"].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart1.ChartAreas["ChartArea1"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;

            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Average time in milliseconds";
            if(Program.watcher!=null)
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: "+Program.watcher.statsRefreshRate + " milliseconds";
            else
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            chart1.GetToolTipText += chart1_GetToolTipText;
            chart1.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Silver;
            chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Silver;
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;

            chart2.ChartAreas["ChartArea1"].AxisX.Interval = 2;
            chart2.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart2.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart2.ChartAreas["ChartArea1"].AxisX.InterlacedColor = Color.Silver;
            chart2.ChartAreas["ChartArea1"].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart2.ChartAreas["ChartArea1"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart2.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
            chart2.ChartAreas["ChartArea1"].AxisY.Title = "Microwatt (uW)";
            chart2.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: "+Program.watcher.statsRefreshRate+" milliseconds";
            chart2.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Silver;
            chart2.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Silver;
            chart2.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart2.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
            chart2.GetToolTipText += chart1_GetToolTipText;

            chart3.ChartAreas["ChartArea1"].AxisX.Interval = 2;
            chart3.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart3.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart3.ChartAreas["ChartArea1"].AxisX.InterlacedColor = Color.Silver;
            chart3.ChartAreas["ChartArea1"].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart3.ChartAreas["ChartArea1"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart3.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
            chart3.ChartAreas["ChartArea1"].AxisY.Title = "Ampere (A)";
            chart3.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: undefined milliseconds";
            chart3.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Silver;
            chart3.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Silver;
            chart3.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart3.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
            chart3.GetToolTipText += chart1_GetToolTipText;

            flowLayoutPanel1.VerticalScroll.Enabled = true;
            flowLayoutPanel1.HorizontalScroll.Enabled = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            for (int i = 0; i < registeredStatistics.Count; i++)
                flowLayoutPanel2.Controls.Add(registeredStatistics[i]);
            UpdateSelectedView();
        }

        public void UpdateSelectedView()
        {

            if (selectedView==1)
            {
                label7.Text = "Average Time";
                panel3.Visible = true;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
            }
            if (selectedView == 2)
            {
                label7.Text = "Current Time";
                panel3.Visible = false;
                panel4.Visible = true;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
            }
            if (selectedView == 3)
            {
                label7.Text = "Average Goal Error";
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = true;
                panel6.Visible = false;
                panel7.Visible = false;
            }
            if (selectedView == 4)
            {
                label7.Text = "Current Goal Error";
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = true;
                panel7.Visible = false;
            }
            if (selectedView == 5)
            {
                label7.Text = "Goal Average Divergence";
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = true;
            }

        }


        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            // Check selected chart element and set tooltip text for it
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("X:\t{0}\nY:\t{1}", dataPoint.XValue, dataPoint.YValues[0]);
                    break;
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutOrchestrationStudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new GUI.About()).ShowDialog();
        }

        private void manageConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void librariesManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new GUI.LibrariesManager()).ShowDialog();
        }

        private void connectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new GUI.ConnectionsManager()).ShowDialog();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startToolStripMenuItem.Text == "Execute")
            {
                try
                {
                    string rep = Program.shellStream.Expect(new Regex(@"[$>]")); //expect user prompt                
                    Program.shellStream.WriteLine("sudo orchestrator");
                    rep = Program.shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        Program.shellStream.WriteLine(Program.password);
                    toolStripStatusLabel5.Text = "Running";
                    toolStripStatusLabel5.ForeColor = Color.Green;
                           Program.watcher.Initialize();
                           Program.watcher.Connect();
                           Program.watcher.Execute();
                 
                    startToolStripMenuItem.Text = "Stop";
                }
                catch (Exception)
                {
                    (new GUI.Error()).ShowDialog();
                }
            }
            else
            {
                Program.watcher.Disconnect();
                toolStripStatusLabel5.Text = "Not Running";
                toolStripStatusLabel5.ForeColor = Color.Red;
                startToolStripMenuItem.Text = "Execute";
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            chart4.Series[0].Color = Color.FromArgb(200,10,20);

            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.HorizontalScroll.Enabled = false;


            chart4.Series[0].IsValueShownAsLabel = false;
     
     


            chart4.ChartAreas["CPU-bIG"].AxisY.Title = "Utilization %";
            chart4.ChartAreas["CPU-bIG"].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart4.ChartAreas["CPU-bIG"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            chart4.ChartAreas["CPU-bIG"].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);

            chart4.ChartAreas["CPU-bIG"].AxisX.LineColor = Color.FromArgb(60,60,60) ;
            chart4.ChartAreas["CPU-bIG"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart4.ChartAreas["CPU-bIG"].AxisX.TitleForeColor = Color.Silver;
            chart4.ChartAreas["CPU-bIG"].AxisY.TitleForeColor = Color.Silver;
            chart4.ChartAreas["CPU-bIG"].AxisX.ScrollBar.Enabled = false;
            chart4.ChartAreas["CPU-bIG"].AxisY.ScrollBar.Enabled = false;
            chart4.ChartAreas["CPU-bIG"].AxisX.ScaleView.Zoom(0, 100);
     
            chart4.ChartAreas["CPU-bIG"].AxisY.Minimum = 0;
            chart4.ChartAreas["CPU-bIG"].AxisY.Maximum = 100;
            chart4.ChartAreas["CPU-bIG"].AxisY.Interval = 25;
            chart4.ChartAreas["CPU-bIG"].AxisY.ScaleView.Zoom(0, 100);
            chart4.ChartAreas["CPU-bIG"].AxisY.ScaleView.SmallScrollSize = 100;


            chart4.ChartAreas["CPU-bIG"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart4.ChartAreas["CPU-bIG"].AxisX.ScaleView.SmallScrollSize = 100;

            chart4.ChartAreas["CPU-Little"].AxisY.Title = "Utilization %";
            chart4.ChartAreas["CPU-Little"].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart4.ChartAreas["CPU-Little"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            chart4.ChartAreas["CPU-Little"].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);

            chart4.ChartAreas["CPU-Little"].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart4.ChartAreas["CPU-Little"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart4.ChartAreas["CPU-Little"].AxisX.TitleForeColor = Color.Silver;
            chart4.ChartAreas["CPU-Little"].AxisY.TitleForeColor = Color.Silver;
            chart4.ChartAreas["CPU-Little"].AxisX.ScrollBar.Enabled = false;
            chart4.ChartAreas["CPU-Little"].AxisY.ScrollBar.Enabled = false;
            chart4.ChartAreas["CPU-Little"].AxisX.ScaleView.Zoom(0, 100);

            chart4.ChartAreas["CPU-Little"].AxisY.Minimum = 0;
            chart4.ChartAreas["CPU-Little"].AxisY.Maximum = 100;
            chart4.ChartAreas["CPU-Little"].AxisY.Interval = 25;
            chart4.ChartAreas["CPU-Little"].AxisY.ScaleView.Zoom(0, 100);
            chart4.ChartAreas["CPU-Little"].AxisY.ScaleView.SmallScrollSize = 100;


            chart4.ChartAreas["CPU-Little"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart4.ChartAreas["CPU-Little"].AxisX.ScaleView.SmallScrollSize = 100;
            chart4.GetToolTipText += chart1_GetToolTipText;


        }

        public void AppExecute(string app,string name,string throughput)
        {
            IDictionary<Renci.SshNet.Common.TerminalModes, uint> termkvp = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
            termkvp.Add(Renci.SshNet.Common.TerminalModes.ECHO, 53);
            ShellStream shellStream = Program.client.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);
            string rep = shellStream.Expect(new Regex(@"[$>]"));
            switch (app)
            {
                case "Video Processing":
                    shellStream.WriteLine("sudo orch-prewitt -n \"" + name+ "\" -i \"/usr/share/orchestrator-prewitt-video/video1.avi\" -o \"/tmp/video1.avi\" -t " + throughput);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                case "Numbers Addition":           
                    shellStream.WriteLine("sudo orch-application -n \"" + name + "\" -t " + throughput+" -b "+1000);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                default:
                    break;
            }
        }

        public void ChangeThroughput(string name,string throughput)
        {
            Program.watcher.sendCommands.Add("throughtput:" + name + ":" + throughput);
        }


        public double convertToValue(string arg)
        {
            double a=0;
            double converter = 1;
            if (arg.Contains("mW")) converter = 1000;
            for (char i = 'A'; i < 'z'; i++)
                   arg= arg.Replace(i, ' ');
            arg = arg.Replace('.', ',');
            arg = arg.Replace('+', ' ');
            arg = arg.Replace('-', ' ');
            arg = arg.Replace(" ", "");

            a = double.Parse(arg);

            return a*converter;
        }



 public void UpdateAppsGADChart(List<Classes.AppData> statsList)
        {
            if (this.chart8.InvokeRequired)
            {
                SetChartApp d = new SetChartApp(UpdateAppsGADChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                int points = maxPoint() + 1;
                for (int i = 0; i < statsList.Count * 2; i = i + 2)
                {
                    if (chart8.Series.IndexOf(statsList[i / 2].name + " - Current") == -1)
                    {
                        chart8.Series.Add(statsList[i / 2].name + " - Current");
                        chart8.Series.Add(statsList[i / 2].name + " - Goal");
                        chart8.Series[statsList[i / 2].name + " - Goal"].ChartType = SeriesChartType.Line;
                        chart8.Series[statsList[i / 2].name + " - Current"].ChartType = SeriesChartType.Line;
                        chart8.Series[statsList[i / 2].name + " - Goal"].BorderWidth = 1;
                        chart8.Series[statsList[i / 2].name + " - Current"].BorderWidth = 2;
                    }
                    chart8.Series[statsList[i / 2].name + " - Current"].Points.AddXY(points, statsList[i / 2].pgoal);
                    chart8.Series[statsList[i / 2].name + " - Goal"].Points.AddXY(points, statsList[i / 2].goal);
                    if (chart8.ChartAreas[0].AxisX.Maximum >= 100)
                        chart8.ChartAreas[0].AxisX.ScaleView.Scroll(chart8.ChartAreas[0].AxisX.Maximum);
                }
                chart8.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }
        public void UpdateAppsCGEChart(List<Classes.AppData> statsList)
        {
            if (this.chart7.InvokeRequired)
            {
                SetChartApp d = new SetChartApp(UpdateAppsCGEChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                int points = maxPoint() + 1;
                for (int i = 0; i < statsList.Count ; i++)
                {
                    if (chart7.Series.IndexOf(statsList[i].name + " - Error") == -1)
                    {
                        chart7.Series.Add(statsList[i ].name + " - Error");
                        chart7.Series[statsList[i ].name + " - Error"].ChartType = SeriesChartType.Line;
                        chart7.Series[statsList[i ].name + " - Error"].BorderWidth = 1;
                    }
                    chart7.Series[statsList[i].name + " - Error"].Points.AddXY(points, statsList[i ].realerror);
                    if (chart7.ChartAreas[0].AxisX.Maximum >= 100)
                        chart7.ChartAreas[0].AxisX.ScaleView.Scroll(chart7.ChartAreas[0].AxisX.Maximum);
                }
                chart7.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }
        public void UpdateAppsAGEChart(List<Classes.AppData> statsList)
        {
            if (this.chart6.InvokeRequired)
            {
                SetChartApp d = new SetChartApp(UpdateAppsAGEChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                int points = maxPoint() + 1;
                for (int i = 0; i < statsList.Count ; i++)
                {
                    if (chart6.Series.IndexOf(statsList[i].name + " - Error") == -1)
                    {
                        chart6.Series.Add(statsList[i ].name + " - Error");
                        chart6.Series[statsList[i ].name + " - Error"].ChartType = SeriesChartType.Line;
                        chart6.Series[statsList[i ].name + " - Error"].BorderWidth = 1;
                    }
                    chart6.Series[statsList[i].name + " - Error"].Points.AddXY(points, statsList[i].error);

                    if (chart6.ChartAreas[0].AxisX.Maximum >= 100)
                        chart6.ChartAreas[0].AxisX.ScaleView.Scroll(chart6.ChartAreas[0].AxisX.Maximum);
                }
                chart6.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }
        public void UpdateAppsCTChart(List<Classes.AppData> statsList)
        {
            if (this.chart5.InvokeRequired)
            {
                SetChartApp d = new SetChartApp(UpdateAppsCTChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                int points = maxPoint() + 1;
                for (int i = 0; i < statsList.Count * 2; i = i + 2)
                {
                    if (chart5.Series.IndexOf(statsList[i / 2].name + " - Current") == -1)
                    {
                        chart5.Series.Add(statsList[i / 2].name + " - Current");
                        chart5.Series.Add(statsList[i / 2].name + " - Goal");
                        chart5.Series[statsList[i / 2].name + " - Goal"].ChartType = SeriesChartType.Line;
                        chart5.Series[statsList[i / 2].name + " - Current"].ChartType = SeriesChartType.Line;
                        chart5.Series[statsList[i / 2].name + " - Goal"].BorderWidth = 1;
                        chart5.Series[statsList[i / 2].name + " - Current"].BorderWidth = 2;
                    }
                    chart5.Series[statsList[i / 2].name + " - Current"].Points.AddXY(points, statsList[i / 2].realcurrent);
                    chart5.Series[statsList[i / 2].name + " - Goal"].Points.AddXY(points, statsList[i / 2].goal);
                    if (chart5.ChartAreas[0].AxisX.Maximum >= 100)
                        chart5.ChartAreas[0].AxisX.ScaleView.Scroll(chart5.ChartAreas[0].AxisX.Maximum);
                }
                chart5.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }







        public void UpdateSysChart(List<Classes.SysData> statsList)
        {
            if (this.chart2.InvokeRequired || this.chart3.InvokeRequired)
            {
                SetChartSys d = new SetChartSys(UpdateSysChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                for (int i = 0; i < statsList.Count; i++)
                {
                    if (statsList[i].name.Contains("POWER"))
                    {
                        if (chart2.Series.IndexOf(statsList[i].name) == -1)
                        {
                            chart2.Series.Add(statsList[i].name);
                            chart2.Series[statsList[i].name].ChartType = SeriesChartType.Line;
                            chart2.Series[statsList[i].name].BorderWidth = 2;
                        }
                        double value= convertToValue(statsList[i].data); 
                        chart2.Series[statsList[i].name].Points.AddXY(chart2.Series[statsList[i].name].Points.Count, value);
                        if (chart2.ChartAreas[0].AxisX.Maximum >= 100)
                            chart2.ChartAreas[0].AxisX.ScaleView.Scroll(chart2.ChartAreas[0].AxisX.Maximum);
                    }
                }
                chart2.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
                for (int i = 0; i < statsList.Count; i++)
                {
                    if (statsList[i].name.Contains("CURR"))
                    {
                        if (chart3.Series.IndexOf(statsList[i].name) == -1)
                        {
                            chart3.Series.Add(statsList[i].name);
                            chart3.Series[statsList[i].name].ChartType = SeriesChartType.Line;
                            chart3.Series[statsList[i].name].BorderWidth = 2;
                        }
                        double value = convertToValue(statsList[i].data);
                        chart3.Series[statsList[i].name].Points.AddXY(chart3.Series[statsList[i].name].Points.Count, value);
                        if (chart3.ChartAreas[0].AxisX.Maximum >= 100)
                            chart3.ChartAreas[0].AxisX.ScaleView.Scroll(chart3.ChartAreas[0].AxisX.Maximum);
                    }
                }
                chart3.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }



        public void UpdateCPUChart(List<Classes.CPUData> cpuList)
        {
            if (this.chart4.InvokeRequired)
            {
                SetChartCPU d = new SetChartCPU(UpdateCPUChart);
                this.Invoke(d, new object[] { cpuList });
            }
            else
            {
                chart4.ChartAreas["CPU-bIG"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
                chart4.ChartAreas["CPU-Little"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
                for (int i = 0; i < cpuList.Count; i++)
                {
                    if (cpuList[i].name.Contains("cpu0"))
                    {
                        double value = convertToValue(cpuList[i].value);
                        chart4.Series[0].Points.AddXY(chart4.Series[0].Points.Count, value);
                        if (chart4.ChartAreas[0].AxisX.Maximum >= 100)
                            chart4.ChartAreas[0].AxisX.ScaleView.Scroll(chart4.ChartAreas[0].AxisX.Maximum);
                    }
                    else
                    {
                        double value = convertToValue(cpuList[i].value);
                        chart4.Series[1].Points.AddXY(chart4.Series[1].Points.Count, value);
                        if (chart4.ChartAreas[1].AxisX.Maximum >= 100)
                            chart4.ChartAreas[1].AxisX.ScaleView.Scroll(chart4.ChartAreas[1].AxisX.Maximum);




                    }
                }
            }
        }


        public int maxPoint()
        {
            try
            {
                int max = chart1.Series[0].Points.Count;
                for (int i = 1; i < chart1.Series.Count; i++)
                {
                    if (chart1.Series[i].Points.Count > max)
                        max = chart1.Series[i].Points.Count;
                }
                return max;
            }catch(Exception)
            {

            }
            return 0;
        }

        public void UpdateAppsChart(List<Classes.AppData> statsList)
        {
            if (this.chart1.InvokeRequired)
            {
                SetChartApp d = new SetChartApp(UpdateAppsChart);
                this.Invoke(d, new object[] { statsList });
            }
            else
            {
                int points = maxPoint() + 1;
                for (int i = 0; i < statsList.Count * 2; i = i + 2)
                {
                    if (chart1.Series.IndexOf(statsList[i / 2].name + " - Current") == -1)
                    {
                        chart1.Series.Add(statsList[i / 2].name + " - Current");
                        chart1.Series.Add(statsList[i / 2].name + " - Goal");
                        chart1.Series[statsList[i / 2].name + " - Goal"].ChartType = SeriesChartType.Line;
                        chart1.Series[statsList[i / 2].name + " - Current"].ChartType = SeriesChartType.Line;
                        chart1.Series[statsList[i / 2].name + " - Goal"].BorderWidth = 1;
                        chart1.Series[statsList[i / 2].name + " - Current"].BorderWidth = 2;
                    }
                    chart1.Series[statsList[i / 2].name + " - Current"].Points.AddXY(points, statsList[i / 2].current);
                    chart1.Series[statsList[i / 2].name + " - Goal"].Points.AddXY(points, statsList[i / 2].goal);
                    if (chart1.ChartAreas[0].AxisX.Maximum >= 100)
                        chart1.ChartAreas[0].AxisX.ScaleView.Scroll(chart1.ChartAreas[0].AxisX.Maximum);
                }
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void chart2_Click_1(object sender, EventArgs e)
        {

        }

        private void Main_Shown(object sender, EventArgs e)
        {
          
                (new GUI.ConnectionsManager()).ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new GUI.Settings()).ShowDialog();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart2.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart2.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart3.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart3.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
        }

        private void chart2_Click_2(object sender, EventArgs e)
        {

        }

        private void chart1_MouseEnter(object sender, EventArgs e)
        {
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = true;
        }

        private void chart2_MouseEnter(object sender, EventArgs e)
        {
            chart2.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = true;
            chart2.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = true;
        }

        private void chart2_MouseLeave(object sender, EventArgs e)
        {
            chart2.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart2.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
        }

        private void chart3_MouseEnter(object sender, EventArgs e)
        {
            chart3.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = true;
            chart3.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = true;
        }

        private void chart3_MouseLeave(object sender, EventArgs e)
        {
            chart3.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart3.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
        }

        private void videoProcessingPrewittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registeredApplications.Add(new UserControl1("Video Processing","video_processing",700));
            flowLayoutPanel1.Controls.Clear();
            foreach( UserControl1 a in registeredApplications)
            {
                flowLayoutPanel1.Controls.Add(a);
            }
           
        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numbersAdditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registeredApplications.Add(new UserControl1("Numbers Addition", "numbers_addition", 700));
            flowLayoutPanel1.Controls.Clear();
            foreach (UserControl1 a in registeredApplications)
            {
                flowLayoutPanel1.Controls.Add(a);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Orchestration Results|*.orc.zst";
            saveFileDialog1.Title = "Save Orchestration Results to";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {

                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                



                fs.Close();
            }

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            if(label2.Text == "Overall Statistics")
            {
                label1.Text = label2.Text;
                label2.Text = "Applications Pool";
                panel2.Hide();
                panel1.Show();
            }
            else
            {
                label1.Text = label2.Text;
                label2.Text = "Overall Statistics";
                panel1.Hide();
                panel2.Show();
            }
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
        }

        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            if (flowLayoutPanel1.VerticalScroll.Visible == false)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 255)
                {
                    splitContainer3.SplitterDistance = splitContainer3.Width - 255;

                }
            }
     
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
           
            if (flowLayoutPanel1.VerticalScroll.Visible == true)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 274)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 274;
            }
         
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
        }

        private void flowLayoutPanel2_SizeChanged(object sender, EventArgs e)
        {
            if (flowLayoutPanel2.VerticalScroll.Visible == true)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 274)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 274;
            }
        }

        private void flowLayoutPanel2_ControlRemoved(object sender, ControlEventArgs e)
        {
            flowLayoutPanel2.HorizontalScroll.Visible = false;
            if (flowLayoutPanel2.VerticalScroll.Visible == false)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 255)
                {
                    splitContainer3.SplitterDistance = splitContainer3.Width - 255;

                }
            }
        }

        private void chart4_MouseEnter(object sender, EventArgs e)
        {
            chart4.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart4.ChartAreas[0].AxisY.ScrollBar.Enabled = true;
            chart4.ChartAreas[1].AxisX.ScrollBar.Enabled = true;
            chart4.ChartAreas[1].AxisY.ScrollBar.Enabled = true;
        }

        private void chart4_MouseLeave(object sender, EventArgs e)
        {
            chart4.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            chart4.ChartAreas[0].AxisY.ScrollBar.Enabled = false;
            chart4.ChartAreas[1].AxisX.ScrollBar.Enabled = false;
            chart4.ChartAreas[1].AxisY.ScrollBar.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            selectedView = 1;
            UpdateSelectedView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            selectedView = 2;
            UpdateSelectedView();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            selectedView = 3;
            UpdateSelectedView();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectedView = 4;
            UpdateSelectedView();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            selectedView = 5;
            UpdateSelectedView();
        }
    }
}
