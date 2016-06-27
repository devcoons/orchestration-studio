using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;
namespace Orchestration_Studio
{
    static class Program
    {
        public static List<Classes.Connection> connections;
        public static GUI.Main main;
        public static SshClient client { get; set; }
        public static ShellStream shellStream { get; set; }
        public static string password = "";
        public static int selectedConnection = -1;
        public static Classes.Watcher watcher;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
          /*
            if(args.Count()<=0)
            {
                ProcessStartInfo info = new ProcessStartInfo(@"" + Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"Update.exe"));
                info.UseShellExecute = true;
                info.Arguments = "\"" + Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"libraries", "lib.update.dll") + "\"";
                try
                {
                    Process.Start(info);
                }
                catch (Win32Exception ex)
                {
                }
                Environment.Exit(0);
            }
             */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            connections = new List<Classes.Connection>();      
            watcher = new Classes.Watcher();
            main = new GUI.Main();
            Application.Run(main);
        }
    }
}
