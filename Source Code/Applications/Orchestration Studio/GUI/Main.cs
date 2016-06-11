using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Orchestration_Studio.GUI
{
    delegate void SetChartApp(List<Classes.AppData> appstats);
    delegate void SetChartSys(List<Classes.SysData> sysstats);
    delegate void SetChartCPU(List<Classes.CPUData> cpustats);
    delegate void ToolTipText(object sender, ToolTipEventArgs e);

    public partial class Main : Form
    {
        public static int selectedView = 0;
        public static int selectedView1 = 0;
        List<GUI.UserControl1> registeredApplications;
        List<GUI.UserControl2> registeredStatistics;


        public void InitializeChart(ref Chart chart)
        {
            chart.ChartAreas["ChartArea1"].AxisX.Interval = 2;
            chart.ChartAreas["ChartArea1"].CursorX.AutoScroll = true;
            chart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas["ChartArea1"].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chart.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(0, 100);
            chart.ChartAreas["ChartArea1"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart.ChartAreas["ChartArea1"].AxisX.ScaleView.SmallScrollSize = 100;
            chart.ChartAreas["ChartArea1"].AxisX.InterlacedColor = Color.Silver;
            chart.ChartAreas["ChartArea1"].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart.ChartAreas["ChartArea1"].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart.ChartAreas["ChartArea1"].CursorY.AutoScroll = true;
            chart.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Silver;
            chart.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Silver;
            chart.ChartAreas["ChartArea1"].AxisX.ScrollBar.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.ScrollBar.Enabled = false;
        }



        public Main()
        {
            InitializeComponent();
            
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
            
            splitContainer3.SplitterDistance = splitContainer3.Width - 287;
            panel1.Hide();

            InitializeChart(ref chart1);
            InitializeChart(ref chart2);
            InitializeChart(ref chart3);
            InitializeChart(ref chart5);
            InitializeChart(ref chart6);
            InitializeChart(ref chart7);
            InitializeChart(ref chart8);


            if (Program.watcher!=null)
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: "+Program.watcher.statsRefreshRate + " milliseconds";
            else
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";

            chart2.ChartAreas["ChartArea1"].AxisY.Title = "Microwatt (uW)";
       
            chart2.GetToolTipText += chart1_GetToolTipText;

        
            chart3.ChartAreas["ChartArea1"].AxisY.Title = "Ampere (A)";
            chart3.GetToolTipText += chart1_GetToolTipText;

            flowLayoutPanel1.VerticalScroll.Enabled = true;
            flowLayoutPanel1.HorizontalScroll.Enabled = false;
            flowLayoutPanel1.VerticalScroll.Visible = true;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            for (int i = 0; i < registeredStatistics.Count; i++)
                flowLayoutPanel2.Controls.Add(registeredStatistics[i]);
            UpdateSelectedView();
            selectedView1 = 1;
            UpdateSelectedView1();


        }

        public void UpdateSelectedView1()
        {
            if(selectedView1==1)
            {
                label8.Text = "Current Power Consumption";
                panel9.Visible = false;
                panel8.Visible = true;

            }
            if(selectedView1==2)
            {
                label8.Text = "Average Power Consumption";

                panel9.Visible = true;
                panel8.Visible = false;

            }

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
                    Program.shellStream.WriteLine("sudo orchestration-service");
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

            selectedView = 1;
            UpdateSelectedView();
            selectedView1 = 1;
            UpdateSelectedView1();


        }

        public void AppExecute(string app,string name,string goal,string min,string max, string profiling,string priority,string policy,string port,string args)
        {
            IDictionary<Renci.SshNet.Common.TerminalModes, uint> termkvp = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
            termkvp.Add(Renci.SshNet.Common.TerminalModes.ECHO, 53);
            ShellStream shellStream = Program.client.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);
            string rep = shellStream.Expect(new Regex(@"[$>]"));
            switch (app)
            {
                case "Video Processing":
                    shellStream.WriteLine("sudo orch-prewitt -n \"" + name+ "\" -i \"/usr/share/orchestrator-prewitt-video/video1.avi\" -o \"/tmp/video1.avi\" -t " + goal);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                case "Numbers Addition":
                    Console.WriteLine("sudo number-addition -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);          
                    shellStream.WriteLine("sudo number-addition -a \"" + name + "\" -g " + goal + " -l "+min+" -h "+max+" -p "+profiling+" -n "+priority+" -r \""+policy+"\" -o "+port+" "+args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                case "Matrix Multiplication":
                    shellStream.WriteLine("sudo orch-mm -n \"" + name + "\" -t " + goal + " -b " + 1000);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                default:
                    break;
            }
        }

        public void ChangeThroughput(string name,string goal,string min,string max)
        {
            Program.watcher.sendCommands.Add("setms:" + name + ":" + goal + ":" + min + ":" + max);

        }
        public void ChangePriority(string name,string priority)
        {
            Program.watcher.sendCommands.Add("setpriority:" + name+":"+priority);
        }

        public void ChangeIndPolicy(string name, string policy)
        {
            Program.watcher.sendCommands.Add("setipolicy:" + name + ":" + policy);
        }

        public void StopApplication(string name)
        {
            Program.watcher.sendCommands.Add("stop:" + name);
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
                        chart8.Series[statsList[i / 2].name + " - Goal"].BorderDashStyle = ChartDashStyle.Dot;
                    }
                    chart8.Series[statsList[i / 2].name + " - Current"].Points.AddXY(points, statsList[i / 2].offset);
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
                    chart7.Series[statsList[i].name + " - Error"].Points.AddXY(points, statsList[i ].errorcurrent);
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
                    chart6.Series[statsList[i].name + " - Error"].Points.AddXY(points, statsList[i].erroraverage);

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
                    chart5.Series[statsList[i / 2].name + " - Current"].Points.AddXY(points, statsList[i / 2].currentms);
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
                        double value = convertToValue(cpuList[i].rtvalue);
                        chart4.Series[0].Points.AddXY(chart4.Series[0].Points.Count, value);
                        if (chart4.ChartAreas[0].AxisX.Maximum >= 100)
                            chart4.ChartAreas[0].AxisX.ScaleView.Scroll(chart4.ChartAreas[0].AxisX.Maximum);
                    }
                    else
                    {
                        double value = convertToValue(cpuList[i].rtvalue);
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
                for (int i = 0; i < statsList.Count * 4; i = i + 4)
                {
                    if (chart1.Series.IndexOf("AV["+statsList[i / 4].name + "]") == -1)
                    {
                        chart1.Palette = ChartColorPalette.None;
                        chart1.Series.Add("AV[" + statsList[i / 4].name + "]");
                        chart1.Series.Add("GL[" + statsList[i / 4].name + "]");
                        chart1.Series.Add("MN[" + statsList[i / 4].name + "]");
                        chart1.Series.Add("MX[" + statsList[i / 4].name + "]");
                        chart1.Series["GL[" + statsList[i / 4].name + "]"].ChartType = SeriesChartType.Line;
                        chart1.Series["AV[" + statsList[i / 4].name + "]"].ChartType = SeriesChartType.Line;
                        chart1.Series["MN[" + statsList[i / 4].name + "]"].ChartType = SeriesChartType.Line;
                        chart1.Series["MX[" + statsList[i / 4].name + "]"].ChartType = SeriesChartType.Line;
                        chart1.Series["GL[" + statsList[i / 4].name + "]"].BorderWidth = 1;
                        chart1.Series["AV[" + statsList[i / 4].name + "]"].BorderWidth = 2;
                        chart1.Series["MN[" + statsList[i / 4].name + "]"].BorderWidth = 1;
                        chart1.Series["MX[" + statsList[i / 4].name + "]"].BorderWidth = 1;
                        chart1.Series["GL[" + statsList[i / 4].name + "]"].BorderDashStyle = ChartDashStyle.Dash;
                        chart1.Series["MN[" + statsList[i / 4].name + "]"].BorderDashStyle = ChartDashStyle.Dot;
                        chart1.Series["MX[" + statsList[i / 4].name + "]"].BorderDashStyle = ChartDashStyle.Dot;
                        chart1.Series["MX[" + statsList[i / 4].name + "]"].Color = chart1.Series["MX[" + statsList[i / 4].name + "]"].Color;
                        chart1.Series["MN[" + statsList[i / 4].name + "]"].Color = chart1.Series["MN[" + statsList[i / 4].name + "]"].Color;
                        chart1.Series["GL[" + statsList[i / 4].name + "]"].Color = chart1.Series["GL[" + statsList[i / 4].name + "]"].Color;
                        chart1.Series["AV[" + statsList[i / 4].name + "]"].Color = chart1.Series["AV[" + statsList[i / 4].name + "]"].Color;



                    }
                    chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].averagems);
                    chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].goal);
                    chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].min);
                    chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].max);
                    if (chart1.ChartAreas[0].AxisX.Maximum >= 100)
                        chart1.ChartAreas[0].AxisX.ScaleView.Scroll(chart1.ChartAreas[0].AxisX.Maximum);
                    SetChartTransparency( chart1, "GL[" + statsList[i / 4].name + "]");
                    SetChartTransparency(chart1, "MX[" + statsList[i / 4].name + "]");
                    SetChartTransparency(chart1, "MN[" + statsList[i / 4].name + "]");
                }
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }
        private void SetChartTransparency(Chart chart, string Seriesname)
        {
            bool setTransparent = true;
            int numberOfPoints = 1;
            chart.ApplyPaletteColors();
            foreach (DataPoint point in chart.Series[Seriesname].Points)
            {
                if (setTransparent)
                    point.Color = Color.FromArgb(0, point.Color);
                else
                    point.Color = Color.FromArgb(255, point.Color);
                numberOfPoints = numberOfPoints - 1;
                if (numberOfPoints == 0)
                {
                    numberOfPoints = 1;
                    setTransparent = !setTransparent;
                }
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
            registeredApplications.Add(new UserControl1("Video Processing","video_processing",700,400,100,0));
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
            registeredApplications.Add(new UserControl1("Numbers Addition", "numAdd"+(new Random()).Next(100,999).ToString(), 700,400,1000,0));
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
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 287)
                {
                    splitContainer3.SplitterDistance = splitContainer3.Width - 287;

                }
            }
     
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
           
            if (flowLayoutPanel1.VerticalScroll.Visible == true)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 303)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 303;
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
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 303)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 303;
            }
        }

        private void flowLayoutPanel2_ControlRemoved(object sender, ControlEventArgs e)
        {
            flowLayoutPanel2.HorizontalScroll.Visible = false;
            if (flowLayoutPanel2.VerticalScroll.Visible == false)
            {
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 287)
                {
                    splitContainer3.SplitterDistance = splitContainer3.Width - 287;

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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            selectedView1 = 1;
            UpdateSelectedView1();
        }

        private void chart2_Click_3(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            selectedView1 = 2;
            UpdateSelectedView1();
        }

        private void matrixMultiplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registeredApplications.Add(new UserControl1("Matrix Multiplication", "appMM", 900,700,1300,0));
            flowLayoutPanel1.Controls.Clear();
            foreach (UserControl1 a in registeredApplications)
            {
                flowLayoutPanel1.Controls.Add(a);
            }
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result != null && result.Object != null)
            {
                if (result.Object is LegendItem)
                {
                    LegendItem legendItem = (LegendItem)result.Object;
                    chart1.Series[legendItem.SeriesName].BorderDashStyle = chart1.Series[legendItem.SeriesName].BorderDashStyle == ChartDashStyle.NotSet ? legendItem.SeriesName[0] == 'A'? ChartDashStyle.Solid : ChartDashStyle.Dash : ChartDashStyle.NotSet;
                }
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
