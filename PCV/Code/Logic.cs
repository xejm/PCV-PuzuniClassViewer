using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

public static class ModLogic
{
    public static List<string> files = new List<string>();
    public static Dictionary<string, List<string>> dictionaryeverything = new Dictionary<string, List<string>>();
    public static Dictionary<Process, string> OutputPrefetchForSingleFile = new Dictionary<Process, string>();
    public static string ExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
    public static string FolderPath = Path.Combine(Path.GetDirectoryName(ExecutablePath), "Puzuni - Class Viewer");
    public static StringBuilder everything = new StringBuilder();
    public static StringBuilder FileHashes = new StringBuilder();


    public static void FolderCreate()
    {
        Directory.CreateDirectory(FolderPath);
    }

    public static void Logic(string basePath)
    {
        try
        {
            SearchAndProcessJars(basePath);
        }
        catch (Exception ex)
        {
        }
    }

    private static void SearchAndProcessJars(string directoryPath)
    {
        foreach (string jarFilePath in Directory.GetFiles(directoryPath, "*.jar", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(jarFilePath);

            files.Add(jarFilePath);

            List<string> classEntries = new List<string>();

            ProcessJarFile(jarFilePath, classEntries);

            dictionaryeverything.Add(fileName, classEntries);
        }
    }

    private static void ProcessJarFile(string jarFilePath, List<string> classEntries)
    {
        void ProcessArchive(ZipArchive archive)
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".class", StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".java", StringComparison.OrdinalIgnoreCase))
                {
                    classEntries.Add(entry.FullName);
                }
                else if (entry.FullName.StartsWith("META-INF/jars/") && entry.FullName.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    using (ZipArchive innerArchive = new ZipArchive(entry.Open()))
                    {
                        ProcessArchive(innerArchive);
                    }
                }
            }
        }


        using (ZipArchive mainArchive = ZipFile.OpenRead(jarFilePath))
        {
            ProcessArchive(mainArchive);

            if (FileHashes.Length == 0)
            {
                FileHashes.AppendLine("File:,SHA-256 Hash:");
            }

            string hash = Hash.CalculateFileHash(jarFilePath);
            FileHashes.AppendLine(Path.GetFileName(jarFilePath) + "," + hash);


        }
    }




    public static void Logic2()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processing 2/5");


        foreach (var kvp in dictionaryeverything)
        {
            string fileName = kvp.Key + ".txt";
            string filePath = Path.Combine(FolderPath, fileName);
            everything.AppendLine($"File: {kvp.Key}");

            if (kvp.Value != null && kvp.Value.Count > 0)
            {
                everything.AppendLine("↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
                everything.AppendLine(string.Join(Environment.NewLine, kvp.Value));
                File.WriteAllLines(filePath, kvp.Value);
            }
            else
            {
                string found0 = "FOUND 0 CLASSES, PLEASE CONFIRM THAT MANUALLY";
                everything.AppendLine(string.Join(Environment.NewLine, found0));
                File.WriteAllLines(filePath, found0.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            }
            everything.AppendLine();



        }
        string everythingfilepath = Path.Combine(FolderPath, "!EVERYTHING.txt");
        string hashesfilepath = Path.Combine(FolderPath, "!HASHES.csv");
        File.WriteAllLines(everythingfilepath, everything.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None));
        File.WriteAllText(hashesfilepath, FileHashes.ToString());


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processing 3/5");
        AntiCheat.HandleAll();

    }


}

