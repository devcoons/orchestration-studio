﻿using Renci.SshNet;
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

            flowLayoutPanel1.VerticalScroll.Visible = true;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            for (int i = 0; i < registeredStatistics.Count; i++)
                flowLayoutPanel2.Controls.Add(registeredStatistics[i]);

            Classes.Helper.SetChartInitState(chart1, "ChartArea1", "Average Elapsed Time (ms)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 2);
            Classes.Helper.SetChartInitState(chart2, "ChartArea1", "Microwatt (uW)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 5);
            Classes.Helper.SetChartInitState(chart3, "ChartArea1", "Ampere (uW)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds",5);
            Classes.Helper.SetChartInitState(chart4, "CPU-bIG", "Utilization %", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 0, 100, 25);
            Classes.Helper.SetChartInitState(chart4, "CPU-Little", "Utilization %", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 0, 100, 25);
            Classes.Helper.SetChartInitState(chart5, "ChartArea1", "Real-Time Elapsed Time (ms)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 2);
            Classes.Helper.SetChartInitState(chart6, "ChartArea1", "Average Goal Error (ms)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 2);
            Classes.Helper.SetChartInitState(chart7, "ChartArea1", "Real-Time Goal Error (ms)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 2);
            Classes.Helper.SetChartInitState(chart8, "ChartArea1", "Absolute Goal Divergence (ms)", "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds", 2);

            chart4.Series[0].Color = Color.FromArgb(200, 10, 20);

            splitContainer3.SplitterDistance = splitContainer3.Width - 287;

            UpdateSelectedView();
            UpdateSelectedView1();
        }

        public void UpdateSelectedView1()
        {
            panel9.Visible = selectedView1 == 0 ? true : false;
            panel9.Visible = selectedView1 == 0 ? false : true;
            label8.Text = selectedView1 == 0 ? "Current Power Consumption" : "Average Power Consumption";
        }

        public void UpdateSelectedView()
        {

            if (selectedView==1)
            {
                label7.Text = "Average Elapsed Time";
                panel3.Visible = true;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
            }
            if (selectedView == 2)
            {
                label7.Text = "Real-Time Elapsed Time";
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
                label7.Text = "Real-Time Goal Error";
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = true;
                panel7.Visible = false;
            }
            if (selectedView == 5)
            {
                label7.Text = "Absolute Goal Divergence";
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = true;
            }
        }


        private void aboutOrchestrationStudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new GUI.About()).ShowDialog();
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
                int points = Classes.Helper.maxPoint(chart8) + 1;
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
                int points = Classes.Helper.maxPoint(chart7) + 1;
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
                int points = Classes.Helper.maxPoint(chart6) + 1;
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
                int points = Classes.Helper.maxPoint(chart5) + 1;
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
                        double value= Classes.Helper.ConvertToValue(statsList[i].data); 
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
                        double value = Classes.Helper.ConvertToValue(statsList[i].data);
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
                        double value = Classes.Helper.ConvertToValue(cpuList[i].rtvalue);
                        chart4.Series[0].Points.AddXY(chart4.Series[0].Points.Count, value);
                        if (chart4.ChartAreas[0].AxisX.Maximum >= 100)
                            chart4.ChartAreas[0].AxisX.ScaleView.Scroll(chart4.ChartAreas[0].AxisX.Maximum);
                    }
                    else
                    {
                        double value = Classes.Helper.ConvertToValue(cpuList[i].rtvalue);
                        chart4.Series[1].Points.AddXY(chart4.Series[1].Points.Count, value);
                        if (chart4.ChartAreas[1].AxisX.Maximum >= 100)
                            chart4.ChartAreas[1].AxisX.ScaleView.Scroll(chart4.ChartAreas[1].AxisX.Maximum);
                    }
                }
            }
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
                int points = Classes.Helper.maxPoint(chart1) + 1;
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
                    if (chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.Count < points - 1)
                        while (chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.Count != points - 1)
                            chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.AddXY(chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.Count, 0);
                    if (chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.Count < points - 1)
                        while (chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.Count != points - 1)
                            chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.AddXY(chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.Count, 0);
                    if (chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.Count < points - 1)
                        while (chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.Count != points - 1)
                            chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.AddXY(chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.Count, 0);
                    if (chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.Count < points - 1)
                        while (chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.Count != points - 1)
                            chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.AddXY(chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.Count, 0);

                    chart1.Series["AV[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].averagems);
                    chart1.Series["GL[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].goal);
                    chart1.Series["MN[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].min);
                    chart1.Series["MX[" + statsList[i / 4].name + "]"].Points.AddXY(points, statsList[i / 4].max);
                    if (chart1.ChartAreas[0].AxisX.Maximum >= 100)
                        chart1.ChartAreas[0].AxisX.ScaleView.Scroll(chart1.ChartAreas[0].AxisX.Maximum);
                    Classes.Helper.SetChartTransparency( chart1, "GL[" + statsList[i / 4].name + "]",2);
                    Classes.Helper.SetChartTransparency(chart1, "MX[" + statsList[i / 4].name + "]",2);
                    Classes.Helper.SetChartTransparency(chart1, "MN[" + statsList[i / 4].name + "]",2);
                }
                chart1.ChartAreas["ChartArea1"].AxisX.Title = "Current Sampling Rate: " + Program.watcher.statsRefreshRate + " milliseconds";
            }
        }
    

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
            Classes.Helper.chartReset(chart1,5);
            Classes.Helper.chartReset(chart2,5);
            Classes.Helper.chartReset(chart3,5);     
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

        private void numbersAdditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registeredApplications.Add(new UserControl1("Numbers Addition", "numAdd"+(new Random()).Next(100,999).ToString(), 700,400,1000,0));
            flowLayoutPanel1.Controls.Clear();
            foreach (UserControl1 a in registeredApplications)
            {
                flowLayoutPanel1.Controls.Add(a);
            }
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
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 287)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 287;
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {  
            if (flowLayoutPanel1.VerticalScroll.Visible == true)
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 303)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 303;         
        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
        }

        private void flowLayoutPanel2_SizeChanged(object sender, EventArgs e)
        {
            if (flowLayoutPanel2.VerticalScroll.Visible == true)
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 303)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 303;
        }

        private void flowLayoutPanel2_ControlRemoved(object sender, ControlEventArgs e)
        {
            flowLayoutPanel2.HorizontalScroll.Visible = false;
            if (flowLayoutPanel2.VerticalScroll.Visible == false)
                if (splitContainer3.SplitterDistance != splitContainer3.Width - 287)
                    splitContainer3.SplitterDistance = splitContainer3.Width - 287;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            selectedView1 = 0;
            UpdateSelectedView1();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            (new Error("Unavailable Feature. Please contact us at isca@teicrete.gr")).ShowDialog();
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

        private void button12_Click(object sender, EventArgs e)
        {
            (new Error("Unavailable Feature. Please contact us at isca@teicrete.gr")).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
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