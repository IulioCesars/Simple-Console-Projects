using System;
using System.Collections.Generic;
using System.Text;

namespace FoldersToZip
{
    public enum TypeMessage { Success, Error, Info }

    public class ConsoleManager
    {
        private ConsoleColor GetTypeMessageColor(TypeMessage typeMessage)
        {
            var color = ConsoleColor.White;

            switch (typeMessage)
            {
                case TypeMessage.Success: { color = ConsoleColor.Green; } break;
                case TypeMessage.Error: { color = ConsoleColor.Red; } break;
                case TypeMessage.Info: { color = ConsoleColor.White; } break;
            }

            return color;
        }

        public void WriteLine(string message, TypeMessage typeMessage = TypeMessage.Info) 
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetTypeMessageColor(typeMessage);

            Console.WriteLine(message);

            Console.ForegroundColor = originalColor;
        }
    }
}
