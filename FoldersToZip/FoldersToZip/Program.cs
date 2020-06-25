using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace FoldersToZip
{
    class Program
    {
        private static ConsoleManager consoleManager;
        private static readonly string[] helpArgs;

        static Program()
        {
            consoleManager = new ConsoleManager();
            helpArgs = new string[] { "-h", "--h", "/h", "-help", "--help", "/help" };
        }

        static void Main(string[] args)
        {
            try
            {
                var containsHelpArg = args.Any(it => helpArgs.Any(x => x.Contains(it, StringComparison.OrdinalIgnoreCase)));
                if (containsHelpArg)
                {
                    ShowHelp();
                    return;
                }

                string sourceFolder = args.ElementAtOrDefault(0);
                string targetFolder = args.ElementAtOrDefault(1);
                string prefix = args.ElementAtOrDefault(2);


                if (!ValidateDirectories(sourceFolder, targetFolder, out string errorMessage))
                {
                    consoleManager.WriteLine(errorMessage, TypeMessage.Error);
                    return;
                }

                var directories = Directory.GetDirectories(sourceFolder);
                var numberOfDirectories = directories.Count();

                for (int i = 0; i < numberOfDirectories; i++)
                {
                    var directory = directories[i];
                    var directoryName = new DirectoryInfo(directory).Name;

                    var filePath = Path.Combine(targetFolder, $"{prefix}{directoryName}.zip");

                    if (File.Exists(filePath))
                    { File.Delete(filePath); }

                    consoleManager.WriteLine($"> Compressing the folder '{directoryName}', ({i + 1}/{numberOfDirectories})");
                    ZipFile.CreateFromDirectory(directory, filePath);
                }

                consoleManager.WriteLine("All subfolders were successfully committed", TypeMessage.Success);
            }
            catch (Exception ex)
            {
                consoleManager.WriteLine("Unexpected Exception: " + ex.GetBaseException().Message, TypeMessage.Error);
            }
        }

        private static void ShowHelp() 
        {
            consoleManager.WriteLine("This program compress all subfolders in zip format");
            consoleManager.WriteLine("- - - - -");
            consoleManager.WriteLine("");
            consoleManager.WriteLine("Arg 1: Source Folder");
            consoleManager.WriteLine("Arg 2: Target Folder");
            consoleManager.WriteLine("Arg 3 (Optional): Prefix for zips");

            var assembly = Assembly.GetExecutingAssembly().GetName();
            consoleManager.WriteLine("");
            consoleManager.WriteLine($"{assembly.Name} - {assembly.Version}");
        }

        private static bool ValidateDirectories(string sourceFolder, string targetFolder, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(sourceFolder))
            {
                errorMessage = "The argument 'source folder' don´t was specified";
                return false;
            }

            if (string.IsNullOrWhiteSpace(targetFolder))
            { targetFolder = sourceFolder; }

            if (!Directory.Exists(sourceFolder))
            {
                errorMessage = "The directory 'source folder' not exists";
                return false;
            }
            else if (Directory.GetDirectories(sourceFolder).Count() == 0)
            {
                errorMessage = "The directory 'source folder' not contains sub folders";
                return false;
            }

            if (!Directory.Exists(targetFolder))
            {
                try
                {
                    Directory.CreateDirectory(targetFolder);
                }
                catch (Exception ex)
                {
                    errorMessage = "The directory 'targer folder' cannot be created, " + ex.GetBaseException().Message;
                    return false;
                }
            }

            return string.IsNullOrWhiteSpace(errorMessage);
        }
    }
}
