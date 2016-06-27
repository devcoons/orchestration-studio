using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.ComponentModel;

namespace Update
{
    static class Program
    {
        public static Type info_class;
        public static Type actions_class;
        public static string[] args;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                Application.Run(new GUI.Error(new Exception("Not enough arguments. Use the application executable.")));
                Environment.Exit(-1);
            }
            Program.args = args;
            try
            {
                if(File.Exists(@"" + args[0]+".upd"))
                {                  
                    Elevate();             
                    if (File.Exists(@"" + args[0]))
                        File.Delete(@"" + args[0]);
                    File.Copy(@"" + args[0] + ".upd", @"" + args[0]);
                    File.Delete(@"" + args[0] + ".upd");
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                info_class = (Assembly.LoadFile(@"" + args[0])).GetType("lib.update.Information");
                actions_class = (Assembly.LoadFile(@"" + args[0])).GetType("lib.update.Actions");
            }
            catch (Exception ex)
            {
                Application.Run(new GUI.Error(ex));
                Environment.Exit(-1);
            }

            Application.Run(new GUI.Main());
            Environment.Exit(0);
        }

        public static void Elevate()
        {
            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) == true)
                return;
            ProcessStartInfo info = new ProcessStartInfo(@""+ Application.ExecutablePath);
            info.UseShellExecute = true;
            info.Verb = "runas";
            info.Arguments ="\""+ Program.args[0]+"\"";
            try
            {
                Process.Start(info);
            }
            catch (Win32Exception ex)
            {
            }
            Environment.Exit(0);    
        }
    }
}
