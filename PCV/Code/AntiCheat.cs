using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

public static class AntiCheat
{
    public static string FolderPath { get; set; } = Path.Combine(ModLogic.FolderPath + "\\Puzuni - AC");
    public static string[] GenericDetections { get; private set; }
    public static string[] ClientDetections { get; private set; }
    public static List<string> ListGenericSummary { get; set; } = new List<string>();
    public static List<string> ListClientSummary { get; set; } = new List<string>();
    public static string Summary { get; set; }
    public static List<string> decryptedLines;
    public static List<string> debug = new List<string>();
    public static string customPIN { get; set; } = "EXAMPLE PIN"; // PIN FOR DETECTIONS
    public static int allSusCount;
    public static int allCount = ModLogic.everything.Length;

    public static void HandleAll()
    {
        GetClientDetections();
        GetGenericDetections();
        CreateFolder();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processing 4/5");
        ScanGenericSummary();
        ScanClientSummary();
        FormatSummary();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processing 5/5");
        GenerateResults();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("\n  [+] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processed");
    }

    public static int WhichPhaseIsIt = 1;
    public static void GetDetections(string url, out string[] detections)
    {
        string backupKey = SafeEncryptor.EncryptionKey;
        SafeEncryptor.EncryptionKey = customPIN;
        try
        {
            List<string> decryptedLines = new List<string>();
            using (var client = new WebClient())
            {
                string content = client.DownloadString(url).Trim();
                string[] lines = content.Split();
                foreach (var line in lines)
                {
                    var decryptedLine = SafeEncryptor.DecryptPin(line);
                    decryptedLines.Add(decryptedLine);
                }
                detections = decryptedLines.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\n  [!] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Error while processing AC: ");
            Thread.Sleep(3500);
            Environment.Exit(0);
            detections = null;
        }
        finally
        {
            SafeEncryptor.EncryptionKey = backupKey;
        }
    }

    public static void GetGenericDetections()
    {
        string[] genericDetections;
        GetDetections("https://GenericDetections.com", out genericDetections);
        GenericDetections = genericDetections;
    }

    public static void GetClientDetections()
    {
        WhichPhaseIsIt++;
        string[] clientDetections;
        GetDetections("https://ClientDetections.com", out clientDetections); 
        ClientDetections = clientDetections;
    }



    public static void CreateFolder()
    {
        Directory.CreateDirectory(FolderPath);
    }

    public static void ScanSummary(string[] detections, List<string> summaryList)
    {
        string everythingAsString = ModLogic.everything.ToString();
        string[] lines = everythingAsString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        int cs = 1;
        foreach (var line in lines)
        {
            if (detections != null)
            {
                foreach (var detection in detections)
                {
                    string[] parts = detection.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                    string detectionDetection = parts[0].Trim();
                    string detectionName = parts[1].Trim();

                    if (line.ToLower().Contains(detectionDetection.ToLower()))
                    {
                        debug.Add(line);
                        allSusCount++;
                        bool isThatTheEnd = false;
                        string detectionNameFormatted = $" [+] {detectionName} [Confidence Score: {cs}]";

                        if (!summaryList.Contains(detectionNameFormatted) && !summaryList.Any(item => item.Contains(detectionName)))
                        {
                            if (!isThatTheEnd)
                            {
                                summaryList.Add(detectionNameFormatted);
                            }
                        }
                        else
                        {
                            isThatTheEnd = true;
                            summaryList.Remove(detectionNameFormatted);
                            cs++;
                            detectionNameFormatted = $" [+] {detectionName} [Confidence Score: {cs}]";
                            summaryList.Add(detectionNameFormatted);
                        }
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n  [!] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Error while processing AC");
                Thread.Sleep(3500);
                Environment.Exit(0);
            }
        }
    }

    public static void ScanGenericSummary()
    {
        ScanSummary(GenericDetections, ListGenericSummary);
    }

    public static void ScanClientSummary()
    {
        ScanSummary(ClientDetections, ListClientSummary);
    }

   
    


    public static void FormatSummary()
    {
        Summary += " .______    __    __   ________   __    __  .__   __.  __          ___       ______ \r\n |   _  \\  |  |  |  | |       /  |  |  |  | |  \\ |  | |  |        /   \\     /      |\r\n |  |_)  | |  |  |  | `---/  /   |  |  |  | |   \\|  | |  |       /  ^  \\   |  ,----'\r\n |   ___/  |  |  |  |    /  /    |  |  |  | |  . `  | |  |      /  /_\\  \\  |  |     \r\n |  |      |  `--'  |   /  /----.|  `--'  | |  |\\   | |  |     /  _____  \\ |  `----.\r\n | _|       \\______/   /________| \\______/  |__| \\__| |__|    /__/     \\__\\ \\______|\r\n                                                                                    ";
        Summary += "\n Detections & Coding by Puzuni | SS -> https://discord.gg/j9Fm6EMtcu";
        Summary += "\n\n Total original CS entries = " + allSusCount.ToString();
        Summary += "\n CS score in per mile = " + ((float)allSusCount / allCount * 1000).ToString("F5") + "‰";
        Summary += "\n ____________________________________________________________________________________";
        Summary += "\n   ___  _  _            _     ___        _             _    _                \r\n  / __|| |(_) ___  _ _ | |_  |   \\  ___ | |_  ___  __ | |_ (_) ___  _ _   ___\r\n | (__ | || |/ -_)| ' \\|  _| | |) |/ -_)|  _|/ -_)/ _||  _|| |/ _ \\| ' \\ (_-<\r\n  \\___||_||_|\\___||_||_|\\__| |___/ \\___| \\__|\\___|\\__| \\__||_|\\___/|_||_|/__/\r\n                                                                             ";
        Summary += "\n";
        if (ListClientSummary.Count > 0)
        {
            Summary += string.Join(Environment.NewLine, ListClientSummary);
        }
        else
        {
            Summary += " [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING]";
        }
        Summary += "\n ____________________________________________________________________________________";
        Summary += "\n   ___                       _       ___        _             _    _                \r\n  / __| ___  _ _   ___  _ _ (_) __  |   \\  ___ | |_  ___  __ | |_ (_) ___  _ _   ___  \r\n | (_ |/ -_)| ' \\ / -_)| '_|| |/ _| | |) |/ -_)|  _|/ -_)/ _||  _|| |/ _ \\| ' \\ (_-<   \r\n  \\___|\\___||_||_|\\___||_|  |_|\\__| |___/ \\___| \\__|\\___|\\__| \\__||_|\\___/|_||_|/__/ ";
        Summary += "\n\n";
        if (ListGenericSummary.Count > 0)
        {
            Summary += string.Join(Environment.NewLine, ListGenericSummary);
        }
        else
        {
            Summary += " [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING] [FOUND NOTHING]";
        }

    }



    public static void GenerateResults()
    {
        File.WriteAllLines(Path.Combine(FolderPath + "\\debug.txt"), debug);
        File.WriteAllText(Path.Combine(FolderPath + "\\AC.txt"), Summary);
    }

}