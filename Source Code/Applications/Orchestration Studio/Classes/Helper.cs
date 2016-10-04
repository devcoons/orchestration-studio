using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Orchestration_Studio.Classes
{
    public static class Helper
    {
        public static void SetChartTransparency(Chart chart, string Seriesname, int numberOfPoints)
        {
            bool setTransparent = true;
            chart.ApplyPaletteColors();
            foreach (DataPoint point in chart.Series[Seriesname].Points)
            {
                point.Color = setTransparent ? Color.FromArgb(0, point.Color): Color.FromArgb(255, point.Color);
                if (--numberOfPoints == 0)
                {
                    numberOfPoints = 1;
                    setTransparent = !setTransparent;
                }
            }
        }

        public static void SetChartInitState(Chart chart)
        {
            chart.GetToolTipText += chart_GetToolTipText;
            chart.MouseEnter += chartArea_MouseEnter;
            chart.MouseLeave += chartArea_MouseLeave;
            chart.MouseClick += chart_MouseClick;
        }

        public static void SetChartInitState(Chart chart, string chartArea, string axisYTitle, string axisXTitle,int axisYInterval)
        {
            chart.ChartAreas[chartArea].AxisY.Title = axisYTitle;
            chart.ChartAreas[chartArea].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart.ChartAreas[chartArea].AxisX.Title = axisXTitle;
            chart.ChartAreas[chartArea].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart.ChartAreas[chartArea].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart.ChartAreas[chartArea].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart.ChartAreas[chartArea].AxisX.TitleForeColor = Color.Silver;
            chart.ChartAreas[chartArea].AxisY.TitleForeColor = Color.Silver;
            chart.ChartAreas[chartArea].AxisX.ScrollBar.Enabled = false;
            chart.ChartAreas[chartArea].AxisY.ScrollBar.Enabled = false;
            chart.ChartAreas[chartArea].AxisX.ScaleView.Zoom(0, 100);
            chart.ChartAreas[chartArea].AxisY.ScaleView.SmallScrollSize = 100;
            chart.ChartAreas[chartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart.ChartAreas[chartArea].AxisX.ScaleView.SmallScrollSize = 100;
            chart.ChartAreas[chartArea].AxisX.Interval = axisYInterval;
            chart.GetToolTipText += chart_GetToolTipText;
            chart.MouseEnter += chartArea_MouseEnter;
            chart.MouseLeave += chartArea_MouseLeave;
            chart.MouseClick += chart_MouseClick;
       //     chart.MouseWheel += new MouseEventHandler(chartArea_MouseWheel);
        }

        public static void SetChartInitState(Chart chart, string chartArea,string axisYTitle, string axisXTitle, int axisYMin, int axisYMax, int axisYInterval)
        {
            chart.ChartAreas[chartArea].AxisY.Title = axisYTitle;
            chart.ChartAreas[chartArea].AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart.ChartAreas[chartArea].AxisX.Title = axisXTitle;
            chart.ChartAreas[chartArea].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular);
            chart.ChartAreas[chartArea].AxisX.LineColor = Color.FromArgb(60, 60, 60);
            chart.ChartAreas[chartArea].AxisX.LineDashStyle = ChartDashStyle.Dot;
            chart.ChartAreas[chartArea].AxisX.TitleForeColor = Color.Silver;
            chart.ChartAreas[chartArea].AxisY.TitleForeColor = Color.Silver;
            chart.ChartAreas[chartArea].AxisX.ScrollBar.Enabled = false;
            chart.ChartAreas[chartArea].AxisY.ScrollBar.Enabled = false;
            chart.ChartAreas[chartArea].AxisX.ScaleView.Zoom(0, 100);
            chart.ChartAreas[chartArea].AxisY.Minimum = axisYMin;
            chart.ChartAreas[chartArea].AxisY.Maximum = axisYMax;
            chart.ChartAreas[chartArea].AxisY.Interval = axisYInterval;
            chart.ChartAreas[chartArea].AxisY.ScaleView.Zoom(0, 100);
            chart.ChartAreas[chartArea].AxisY.ScaleView.SmallScrollSize = 100;
            chart.ChartAreas[chartArea].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart.ChartAreas[chartArea].AxisX.ScaleView.SmallScrollSize = 100;
            chart.GetToolTipText += chart_GetToolTipText;
            chart.MouseEnter += chartArea_MouseEnter;
            chart.MouseLeave += chartArea_MouseLeave;
            chart.MouseClick += chart_MouseClick;
        }

        public static double ConvertToValue(string arg)
        {
            double a = 0;
            double converter = 1;
            if (arg.Contains("mW")) converter = 1000;
            for (char i = 'A'; i < 'z'; i++)
                arg = arg.Replace(i, ' ');
            arg = arg.Replace(',', '.');
            arg = arg.Replace('+', ' ');
            arg = arg.Replace('-', ' ');
            arg = arg.Replace(" ", "");
            a = double.Parse(arg, CultureInfo.InvariantCulture);
            return a * converter;
        }

        public static void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format("X:\t{0}\nY:\t{1}", dataPoint.XValue, dataPoint.YValues[0]);
                    break;
            }
        }

        public static void chartArea_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            foreach (ChartArea chartArea in chart.ChartAreas)
            {
                try
                {
                    chartArea.AxisX.ScaleView.SmallScrollSize = 100;
                    if (e.Delta< 0)
                    {
                        Console.WriteLine("-1");
                        chartArea.AxisX.Minimum = chartArea.AxisX.Minimum - 10 >= 0 ? chartArea.AxisX.Minimum - 10 : 0;
                        chartArea.AxisX.Maximum = maxPoint(chart) <= 100 ? 100 : maxPoint(chart);
                        chartArea.AxisX.ScaleView.Zoom(chartArea.AxisX.Minimum, chartArea.AxisX.Maximum);
                        chartArea.AxisX.ScaleView.Scroll(chartArea.AxisX.Maximum);
                    }

                    if (e.Delta > 0)
                    {
                        chartArea.AxisX.Minimum = maxPoint(chart) - 100 >= 0 ? maxPoint(chart) - 100 : 0;
                        chartArea.AxisX.Maximum = maxPoint(chart) <= 100 ? 100 : maxPoint(chart);
                        chartArea.AxisX.ScaleView.Zoom(chartArea.AxisX.Minimum, chartArea.AxisX.Maximum);
                        chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
                        chartArea.AxisX.ScrollBar.Enabled = false;
                        chartArea.AxisY.ScrollBar.Enabled = false;
                        chartArea.AxisX.ScaleView.Scroll(chartArea.AxisX.Maximum);
                    }                 
                }
                catch { }
            }
        }

        public static void chartArea_MouseEnter(object sender, EventArgs e)
        {
            Chart chart = (Chart)sender;
            chart.Focus();
            foreach (ChartArea chartArea in chart.ChartAreas)
            {
                chartArea.AxisX.ScrollBar.Enabled = true;
                chartArea.AxisY.ScrollBar.Enabled = true;
            }
        }

        public static void chartArea_MouseLeave(object sender, EventArgs e)
        {
            Chart chart = (Chart)sender;
            chart.Parent.Focus();
            foreach (ChartArea chartArea in chart.ChartAreas)
            {
                chartArea.AxisX.ScrollBar.Enabled = false;
                chartArea.AxisY.ScrollBar.Enabled = false;
            }       
        }

        public static void chart_MouseClick(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            HitTestResult result = chart.HitTest(e.X, e.Y);
            if (result != null && result.Object != null)
            {
                if (result.Object is LegendItem)
                {
                    LegendItem legendItem = (LegendItem)result.Object;
                    chart.Series[legendItem.SeriesName].BorderDashStyle = chart.Series[legendItem.SeriesName].BorderDashStyle == ChartDashStyle.NotSet ? legendItem.SeriesName[0] == 'A' ? ChartDashStyle.Solid : ChartDashStyle.Dash : ChartDashStyle.NotSet;
                }
            }
        }

        public static void chartReset(Chart chart,int axisYInterval)
        {
            chart.Series.Clear();
            foreach (ChartArea chartArea in chart.ChartAreas)
            {
                chartArea.AxisX.ScrollBar.Enabled = false;
                chartArea.AxisY.ScrollBar.Enabled = false;
                chartArea.AxisX.ScaleView.Zoom(0, 100);
                chartArea.AxisY.ScaleView.SmallScrollSize = 100;
                chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
                chartArea.AxisX.ScaleView.SmallScrollSize = 100;
                chartArea.AxisX.Interval = axisYInterval;
            }
        }

        public static int maxPoint(Chart chart)
        {
            try
            {
                int max = chart.Series[0].Points.Count;
                for (int i = 1; i < chart.Series.Count; i++)
                {
                    if (chart.Series[i].Points.Count > max)
                        max = chart.Series[i].Points.Count;
                }
                return max;
            }
            catch (Exception)
            {
            }
            return 0;
        }

    }
}
