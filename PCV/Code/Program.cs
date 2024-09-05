




using System;
using System.IO;

public class Program
{
    public static string minecraftModsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\mods");
    public static string currentPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
    public static bool IsPathChoosen = false;
    public static string finalDecision;
    public static string currentVersion { get; private set; } = "PCV V2.3";

    public static void DefaultText()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("   ___                        _                            \r\n  | _ \\ _  _  ___ _  _  _ _  (_)                           \r\n  |  _/| || ||_ /| || || ' \\ | |                           \r\n  |_|   \\_,_|/__| \\_,_||_||_||_|                           \r\n    ___  _                __   __ _                        \r\n   / __|| | __ _  ___ ___ \\ \\ / /(_) ___ __ __ __ ___  _ _ \r\n  | (__ | |/ _` |(_-<(_-<  \\ V / | |/ -_)\\ V  V // -_)| '_|\r\n   \\___||_|\\__,_|/__//__/   \\_/  |_|\\___| \\_/\\_/ \\___||_|  \r\n                                                           ");
        Console.ResetColor();
        Console.Write("\n Developed by ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("PuzuniSS");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" ->");
        Console.ResetColor();
        Console.Write(" xame_");
        Console.Write("\n Discord ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("-> ");
        Console.ResetColor();
        Console.Write("https://discord.gg/JnhhF9hG8z\n");
        Console.WriteLine("—————————————————————————————————————————————————————————————————");
    }

    public static void Main()
    {
        Console.SetWindowSize(65, 22);
        Console.SetBufferSize(65, 22);

        DefaultText();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" Console:");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Initializing");
        Console.ResetColor();



        while (!IsPathChoosen)
        {
            Console.Clear();
            DefaultText();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  Select path option:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n  1.");
            Console.ResetColor();
            Console.Write(" Current executable path");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n  2.");
            Console.ResetColor();
            Console.Write(" " + minecraftModsPath + "\n");
            string PathChoosen = Console.ReadLine();

            if (PathChoosen == "1")
            {
                finalDecision = currentPath;
                VoidAfterPath();
            }
            if (PathChoosen == "2")
            {
                finalDecision = minecraftModsPath;
                VoidAfterPath();
            }
            else
            {
            }

        }



    }

    public static void VoidAfterPath()
    {
        IsPathChoosen = true;
        Console.Clear();
        DefaultText();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("  [/] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Processing 1/5");
        ModLogic.FolderCreate();
        ModLogic.Logic(finalDecision);
        ModLogic.Logic2();
        Console.Write("\n\n");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
    }
}



