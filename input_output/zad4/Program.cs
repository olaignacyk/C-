using System;

class Program
{
    static void Main(string[] args)
    {

        string[] sounds = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H" };

        Console.Write("Podaj nazwę dźwięku (np. C, C#, D): ");
        string userInput = Console.ReadLine().ToUpper(); 

        int startIndex = Array.IndexOf(sounds, userInput); 

        if (startIndex == -1) 
        {
            Console.WriteLine("Podano niepoprawną nazwę dźwięku.");
            return;
        }

        Console.Write("Gama dur dla " + sounds[startIndex] + "-dur: ");
        Console.Write(sounds[startIndex] + " ");
        int[] intervals = { 2, 2, 1, 2, 2, 2, 1 }; 
        int index = startIndex;
        for (int i = 0; i < intervals.Length; i++)
        {
            index = (index + intervals[i]) % sounds.Length;
            Console.Write(sounds[index] + " ");
        }
        Console.WriteLine();
    }
}
