using System;
using System.Collections.Generic;
using System.Threading;

class Obiekt
{
    public int NumerWatku;
    public Obiekt(int n)
    {
        NumerWatku = n;
    }
}

class Producent
{
    public int Numer;
    public int Opoznienie;
    private readonly List<Obiekt> sharedBuffer;
    private readonly Random random = new Random();
    public bool Endme = false;

    public Producent(int Numer_, int Opoznienie, List<Obiekt> sharedBuffer)
    {
        this.Numer = Numer_;
        this.Opoznienie = Opoznienie;
        this.sharedBuffer = sharedBuffer;
    }

    public void Start()
    {
        while (!Endme)
        {
            Obiekt obiekt = new Obiekt(this.Numer);
            lock (sharedBuffer)
            {
                sharedBuffer.Add(obiekt);
                Console.WriteLine($"Producent {Numer} wygenerował obiekt. Liczba obiektów w buforze: {sharedBuffer.Count}");
            }
            Thread.Sleep(random.Next(this.Opoznienie));
        }
    }
}

class Konsument
{
    private readonly List<Obiekt> sharedBuffer;
    private readonly Random random = new Random();
    public bool Endme = false;
    private readonly List<int> counter;

    public Konsument(List<Obiekt> sharedBuffer,List<int> counter)
    {
        this.sharedBuffer = sharedBuffer;
        this.counter=counter;
    }


    public void Start()
    {
        while (!Endme)
        {
            lock (sharedBuffer)
            {
                if (sharedBuffer.Count > 0)
                {
                    Obiekt obiekt = sharedBuffer[0];
                    sharedBuffer.RemoveAt(0);
                    Console.WriteLine($"Konsument pobrał obiekt od producenta {obiekt.NumerWatku}. Liczba obiektów w buforze: {sharedBuffer.Count}");
                    counter[obiekt.NumerWatku]++;
                }
            }
            Thread.Sleep(random.Next(1000)); // Losowe opóźnienie dla konsumenta
        }
    }
}