using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <text1> <text2> ... <textN> <repeatCount>");
                return;
            }

            int repeatCount;
            if (!int.TryParse(args[^1], out repeatCount))
            {
                Console.WriteLine("Last parameter must be an integer representing repeat count.");
                return;
            }

            for (int i = 0; i < args.Length - 1; i++)
            {
                for (int j = 0; j < repeatCount; j++)
                {
                    Console.WriteLine(args[i]);
                }
            }
        }
    }
}
