using System;
using System.IO;

namespace YtdlpUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string customPath = Prompt("Enter a custom path for yt-dlp.exe (leave blank to skip): ");
            string neosDir = @"C:\Neos\app";
            string steamDir = @"C:\Program Files\Steam\steamapps\common\NeosVR";
            string[] targetDirs;

            if (!string.IsNullOrEmpty(customPath))
            {
                Console.WriteLine($"Updating yt-dlp.exe at custom path: {customPath}");
                UpdateYtdlp(customPath);
            }
            else
            {
                string updateBoth = Prompt("Update yt-dlp.exe in both NeosVR and Steam directories? [Y/N]: ");
                if (updateBoth.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    targetDirs = new string[] { Path.Combine(neosDir, "RuntimeData"), Path.Combine(steamDir, "RuntimeData") };
                    UpdateYtdlpAll(targetDirs);
                }
                else if (updateBoth.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    string updateNeos = Prompt("Update yt-dlp.exe in NeosVR directory? [Y/N]: ");
                    if (updateNeos.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        targetDirs = new string[] { Path.Combine(neosDir, "RuntimeData") };
                        UpdateYtdlpAll(targetDirs);
                    }
                    string updateSteam = Prompt("Update yt-dlp.exe in Steam directory? [Y/N]: ");
                    if (updateSteam.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        targetDirs = new string[] { Path.Combine(steamDir, "RuntimeData") };
                        UpdateYtdlpAll(targetDirs);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Aborting update.");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        static void UpdateYtdlpAll(string[] targetDirs)
        {
            foreach (string targetDir in targetDirs)
            {
                if (File.Exists(Path.Combine(targetDir, "yt-dlp.exe")))
                {
                    Console.WriteLine($"Updating yt-dlp.exe in {targetDir}");
                    Directory.SetCurrentDirectory(targetDir);
                    System.Diagnostics.Process.Start("yt-dlp.exe", "-U").WaitForExit();
                    Console.WriteLine($"yt-dlp.exe in {targetDir} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"yt-dlp.exe not found in {targetDir}. Skipping update.");
                }
            }
        }

        static void UpdateYtdlp(string targetDir)
        {
            if (File.Exists(Path.Combine(targetDir, "yt-dlp.exe")))
            {
                Console.WriteLine($"Updating yt-dlp.exe in {targetDir}");
                Directory.SetCurrentDirectory(targetDir);
                System.Diagnostics.Process.Start("yt-dlp.exe", "-U").WaitForExit();
                Console.WriteLine($"yt-dlp.exe in {targetDir} updated successfully.");
            }
            else
            {
                Console.WriteLine($"yt-dlp.exe not found in {targetDir}. Skipping update.");
            }
        }
    }
}