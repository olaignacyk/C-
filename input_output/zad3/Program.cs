using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Nie podano nazwy pliku jako argumentu.");
            return;
        }

        string fileName = args[0];

        if (!File.Exists(fileName))
        {
            Console.WriteLine("Podany plik nie istnieje.");
            return;
        }

        int maxNumber = int.MinValue;
        int lineNumber = 0;
        int maxLineNumber = 0;

        using (StreamReader sr = new StreamReader(fileName))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                lineNumber++;

                if (int.TryParse(line, out int number))
                {
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                        maxLineNumber = lineNumber;
                    }
                }
            }
        }

        if (maxNumber != int.MinValue)
        {
            Console.WriteLine($"Największa liczba: {maxNumber}, znaleziona w linijce: {maxLineNumber}");
        }
        else
        {
            Console.WriteLine("Brak liczb w pliku.");
        }
    }
}
