using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orchestration_Studio.Classes
{
    public class Watcher
    {
        public volatile bool isActive = true;
        public volatile Socket socket;
        public volatile IPAddress ipAddress;
        public volatile IPEndPoint remoteEP;
        public List<string> sendCommands;
        public volatile int statsRefreshRate = 500;
        public volatile int loopRefreshRate = 25000;
        public volatile string globalPolicy = "- None -";
        public volatile string globalPolicyParameters = "";


        public volatile BackgroundWorker Worker;


        public void Initialize()
        {
            sendCommands = new List<string>();
            Worker = new BackgroundWorker();
            Worker.DoWork += (new WorkersActions()).Worker;
        }

        public void Connect()
        {
            ipAddress = IPAddress.Parse(Program.connections[Program.selectedConnection].host);
            remoteEP = new IPEndPoint(ipAddress, int.Parse(Program.connections[Program.selectedConnection].port));
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(remoteEP);
                if (socket.Connected)
                {
                    Program.main.toolStripStatusLabel6.Text = "Connected";
                    Program.main.toolStripStatusLabel6.ForeColor = Color.Green;
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        public void Execute()
        {
            if (socket.Connected)
            {
                Worker.RunWorkerAsync(this);
                return;
            }
        }

        public void Disconnect()
        {
            isActive = false;
            Thread.Sleep(75);
            socket.Send(Encoding.ASCII.GetBytes("terminate"));
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Program.main.toolStripStatusLabel6.Text = "Disconnected";
            Program.main.toolStripStatusLabel6.ForeColor = Color.Red;
        }

        private class WorkersActions
        {
            public void Worker(object sender, DoWorkEventArgs e)
            {
                byte[] bytes = new byte[2048];
                int bytesRec;
                while (((Watcher)e.Argument).isActive)
                {
                    ((Watcher)e.Argument).socket.Send(Encoding.ASCII.GetBytes("appstats"));
                    bytesRec = ((Watcher)e.Argument).socket.Receive(bytes);
                    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec).Substring(0, bytesRec));
                    try
                    {
                        List<AppData> appStatSet = JsonConvert.DeserializeObject<List<AppData>>(Encoding.ASCII.GetString(bytes, 0, bytesRec).Substring(0, bytesRec));
                        if (appStatSet.Count != 0)
                        {
                            Program.main.UpdateAppsChart(appStatSet);
                            Program.main.UpdateAppsCTChart(appStatSet);
                            Program.main.UpdateAppsAGEChart(appStatSet);
                            Program.main.UpdateAppsCGEChart(appStatSet);
                            Program.main.UpdateAppsGADChart(appStatSet);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(((Watcher)e.Argument).statsRefreshRate / 4);


                    ((Watcher)e.Argument).socket.Send(Encoding.ASCII.GetBytes("sysstats"));
                    try
                    {
                        bytesRec = ((Watcher)e.Argument).socket.Receive(bytes);
                        List<SysData> sysStatSet = JsonConvert.DeserializeObject<List<SysData>>(Encoding.ASCII.GetString(bytes, 0, bytesRec).Substring(0, bytesRec));
                        if (sysStatSet.Count != 0)
                        {
                            Program.main.UpdateSysChart(sysStatSet);
                        }
                    }
                    catch (Exception)
                    {

                    }

                    Thread.Sleep(((Watcher)e.Argument).statsRefreshRate / 4);

                    if(((Watcher)e.Argument).sendCommands.Count>0)
                    {
                        Console.WriteLine(((Watcher)e.Argument).sendCommands[0]);
                        ((Watcher)e.Argument).socket.Send(Encoding.ASCII.GetBytes(((Watcher)e.Argument).sendCommands[0]));
                        ((Watcher)e.Argument).sendCommands.RemoveAt(0);
                    }


                    Thread.Sleep(((Watcher)e.Argument).statsRefreshRate / 4);

                    ((Watcher)e.Argument).socket.Send(Encoding.ASCII.GetBytes("cpuusage"));
                    try
                    {
                        bytesRec = ((Watcher)e.Argument).socket.Receive(bytes);
                        List<CPUData> cpuStatSet = JsonConvert.DeserializeObject<List<CPUData>>(Encoding.ASCII.GetString(bytes, 0, bytesRec).Substring(0, bytesRec));
                        if (cpuStatSet.Count != 0)
                        {
                            Program.main.UpdateCPUChart(cpuStatSet);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(((Watcher)e.Argument).statsRefreshRate / 4);

                }
            }
        }
    }
}

