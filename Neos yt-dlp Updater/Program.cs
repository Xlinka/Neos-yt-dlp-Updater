using System;
using System.Globalization;
using System.IO;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Resources;

namespace YtdlpUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //code for debugging 

            //var localeFolder = Path.Combine(Directory.GetCurrentDirectory(), "Locale");
            //Console.WriteLine($"Searching for locales in {localeFolder}");

            //var currentLocale = CultureInfo.CurrentCulture.Name;
            //var localeFilePath = Path.Combine(localeFolder, $"{currentLocale}.json");
            //Console.WriteLine($"Looking for locale file: {localeFilePath}");

            //if (!File.Exists(localeFilePath))
            //{
            //    Console.WriteLine($"Locale file for {currentLocale} not found.");
            //    return;
            //}

            var localeFolder = Path.Combine(Directory.GetCurrentDirectory(), "Locale");
            var currentLocale = CultureInfo.CurrentCulture.Name;
            var localeFilePath = Path.Combine(localeFolder, $"{currentLocale}.json");
            if (!File.Exists(localeFilePath))
            {
                Console.WriteLine($"Locale file for {currentLocale} not found.");
                return;
            }

            var langFile = File.ReadAllText(localeFilePath);

            var langData = JsonSerializer.Deserialize<Dictionary<string, string>>(langFile);

            string customPath = Prompt(langData["custom_path"]);
            string neosDir = langData["neos_dir"];
            string steamDir = langData["steam_dir"];
            string[] targetDirs;

            if (!string.IsNullOrEmpty(customPath))
            {
                Console.WriteLine($"{langData["updating_yt_dlp_at"]} {customPath}");
                UpdateYtdlp(customPath, langData);
            }
            else
            {
                string updateBoth = Prompt(langData["update_both"]);
                if (updateBoth.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    targetDirs = new string[] { Path.Combine(neosDir, "RuntimeData"), Path.Combine(steamDir, "RuntimeData") };
                    UpdateYtdlpAll(targetDirs, langData);
                }
                else if (updateBoth.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    string updateNeos = Prompt(langData["update_neos"]);
                    if (updateNeos.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        targetDirs = new string[] { Path.Combine(neosDir, "RuntimeData") };
                        UpdateYtdlpAll(targetDirs, langData);
                    }
                    string updateSteam = Prompt(langData["update_steam"]);
                    if (updateSteam.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        targetDirs = new string[] { Path.Combine(steamDir, "RuntimeData") };
                        UpdateYtdlpAll(targetDirs, langData);
                    }
                }
                else
                {
                    Console.WriteLine(langData["invalid_input"]);
                }
            }

            Console.WriteLine(langData["exit_prompt"]);
            Console.ReadKey();
        }

        static string Prompt(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static void UpdateYtdlpAll(string[] targetDirs, Dictionary<string, string> langData)
        {
            foreach (string targetDir in targetDirs)
            {
                if (File.Exists(Path.Combine(targetDir, "yt-dlp.exe")))
                {
                    Console.WriteLine(langData["updating_ytdlp"], targetDir);
                    Directory.SetCurrentDirectory(targetDir);
                    System.Diagnostics.Process.Start("yt-dlp.exe", "-U").WaitForExit();
                    Console.WriteLine(langData["update_successful"], targetDir);
                }
                else
                {
                    Console.WriteLine(langData["ytdlp_not_found"], targetDir);
                }
            }
        }

        static void UpdateYtdlp(string customPath, Dictionary<string, string> langData)
        {
            if (File.Exists(Path.Combine(customPath, "yt-dlp.exe")))
            {
                Console.WriteLine(langData["updating_ytdlp"], customPath);
                Directory.SetCurrentDirectory(customPath);
                System.Diagnostics.Process.Start("yt-dlp.exe", "-U").WaitForExit();
                Console.WriteLine(langData["update_successful"], customPath);
            }
            else
            {
                Console.WriteLine(langData["ytdlp_not_found"], customPath);
            }
        }
    }
}