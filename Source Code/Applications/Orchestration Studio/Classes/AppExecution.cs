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
        public static void Execute(string app, string name, string goal, string min, string max, string profiling, string priority, string policy, string port, string args)
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
                case "Matrix Multiplication":
                    shellStream.WriteLine("sudo matrix-multiplication -a \"" + name + "\" -g " + goal + " -l " + min + " -h " + max + " -p " + profiling + " -n " + priority + " -r \"" + policy + "\" -o " + port + " " + args);
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
