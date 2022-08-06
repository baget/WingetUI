using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WingetUI
{
    public partial class Winget
    {
        const string WINGET_UPGRADE_CMD = "upgrade";
        const string WINGET_NAME = "winget.exe";
        const string HEADER_REGEX = "Name.*Id.*Version.*";
        public Winget()
        {

        }

        public static IList<Entity> GetUpdates()
        {
            List<Entity> updates = new List<Entity>();

            var exitcode = RunWingetCmd(WINGET_UPGRADE_CMD, out string output);

            if (exitcode != 0)
            {
                return updates;
            }

            // Filter-out unicode chars
            output = Regex.Replace(output, @"[^\u0000-\u007F]+", " ");

            var lines = output.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var inx = FindHeader(lines);

            if (inx.HasValue)
            {
                // Get Only Update, Remove 2 Lines of Headers and 2 Lines of Fotter
                for (long i = inx.Value + 2; i < lines.LongLength - 2; i++)
                {
                    Winget.Entity entity = new()
                    {
                        Name = lines[i].Substring(0, 49).Trim(),
                        Id   = lines[i].Substring(49, 34).Trim(),
                        Version = lines[i].Substring(83, 17).Trim(),
                        AvailableVersion = lines[i].Substring(100, 13).Trim(),
                        Source = lines[i].Substring(113).Trim(),
                };

                    updates.Add(entity);
                }
            }

            return updates;
        }

        private static long? FindHeader(string[] lines)
        {
            var regex = new Regex(HEADER_REGEX, RegexOptions.Compiled | RegexOptions.Singleline);

            for (long i = 0; i < lines.LongLength; i++)
            {
                if (regex.IsMatch(lines[i]))
                {
                    return i;
                }
            }

            return null;
        }

        public static Boolean UpgradeSoftware(Winget.Entity entity)
        {
            return UpgradeSoftware(entity.Id);
        }
        
        public static Boolean UpgradeSoftware(string id)
        {
            var cmd = $"{WINGET_UPGRADE_CMD} --id \"{id}\"";

            var exitCode = RunWingetCmd(cmd, out string _output);

            return exitCode == 0;
        }

        private static int RunWingetCmd(string args, out string output)
        {
            var startInfo  = new ProcessStartInfo(WINGET_NAME);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            if (!String.IsNullOrEmpty(args))
            {
                startInfo.Arguments = args;
            }

            var process = Process.Start(startInfo);
            if (process == null)
            {
                output = String.Empty;
                return -1;
            }
           
            output = process.StandardOutput.ReadToEnd();
            return process.ExitCode;
        }
    }
}
