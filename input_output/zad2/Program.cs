using System;

namespace MyConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
        StreamWriter sw = new StreamWriter("Wynik.txt");

        int suma = 0;
        int iloscLiczb = 0;

        Console.WriteLine("Wpisuj liczby (wpisz 0 aby zakończyć):");

        while (true){
            int liczba=Convert.ToInt32(Console.ReadLine());

            if (liczba==0) break;

            suma+=liczba;
            iloscLiczb+=1;
        }
        double srednia= (double)suma/iloscLiczb;

        Console.WriteLine("Suma: " + suma);
        Console.WriteLine("Średnia: " + srednia);

        sw.WriteLine("Suma: " + suma);
        sw.WriteLine("Średnia: " + srednia);

        sw.Close();
        }
    }
}
