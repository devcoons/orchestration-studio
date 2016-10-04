using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Orchestration_Studio.Classes
{
    public static class AppExecution
    {
        public static void Execute(string app, string name, string goal, string min, string max, string profiling, string priority, string policy, string port, string args,string custom_arg1, string custom_arg2, string custom_arg3)
        {
            IDictionary<Renci.SshNet.Common.TerminalModes, uint> termkvp = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
            termkvp.Add(Renci.SshNet.Common.TerminalModes.ECHO, 53);
            ShellStream shellStream = Program.client.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);
            string rep = shellStream.Expect(new Regex(@"[$>]"));
            switch (app)
            {
                case "Video Processing":
                    shellStream.WriteLine("sudo orch-prewitt -n \"" + name + "\" -i \"/usr/share/orchestrator-prewitt-video/video1.avi\" -o \"/tmp/video1.avi\" -t " + goal);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                case "Numbers Addition":
                    shellStream.WriteLine("sudo number-addition -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                case "Non Numbers Addition":
                    shellStream.WriteLine("sudo non-number-addition -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;

                case "Matrix Multiplication":
                    shellStream.WriteLine("sudo matrix-multiplication-on2 -c \""+custom_arg1+ "\" -b \"" + custom_arg2+ "\" -d \"" + custom_arg3+ "\" -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;

                case "Image Processing Sobel":

                    if (custom_arg1 == "XS - 75x75")
                        custom_arg1 = "image_xs";
                    if (custom_arg1 == "XM - 200x200")
                        custom_arg1 = "image_xm";
                    if (custom_arg1 == "CM - 500x500")
                        custom_arg1 = "image_cm";
                    if (custom_arg1 == "HD - 1280x720")
                        custom_arg1 = "image_hd";
                    shellStream.WriteLine("sudo image-sobel-on2 -b \"" + custom_arg1 + "\" -c \"" + custom_arg2 + "\" -d \"" + custom_arg3 + "\" -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                                       
                case "Workload Maker":
                    shellStream.WriteLine("sudo workload-maker " + args);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                    if (rep.Contains(":"))
                        shellStream.WriteLine(Program.password);
                    break;
                default:
                    break;
            }
        }
    }
}
