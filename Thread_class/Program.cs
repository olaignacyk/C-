
class Program
{
    public static void Zadanie1(){
        Random random = new Random(Environment.TickCount);
        Console.WriteLine("Podaj liczbę producentów:");
        int n = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Podaj liczbę konsumentów:");
        int m = Convert.ToInt32(Console.ReadLine());

        List<Obiekt> sharedBuffer = new List<Obiekt>();
        List<int> counter = new List<int>();
        for (int i = 0; i < n; i++)
        {
            counter.Add(0);
        }


        List<Producent> producenci = new List<Producent>();
        for (int i = 0; i < n; i++)
        {
            Producent producent = new Producent(i, random.Next(1000), sharedBuffer);
            producenci.Add(producent);
            Thread producentThread = new Thread(new ThreadStart(producent.Start));
            producentThread.Start();
        }

        List<Konsument> konsumenci = new List<Konsument>();
        for (int i = 0; i < m; i++)
        {
            Konsument konsument = new Konsument(sharedBuffer, counter);
            konsumenci.Add(konsument);
            Thread konsumentThread = new Thread(new ThreadStart(konsument.Start));
            konsumentThread.Start();
        }

        Console.WriteLine("Naciśnij klawisz 'q', aby zakończyć.");
        while (Console.ReadKey().Key != ConsoleKey.Q)
        {
            // Czekaj na klawisz 'q'
        }

        // Zatrzymywanie wątków
        Console.WriteLine("Zatrzymywanie wątków...");

        foreach (Producent producent in producenci)
        {
            producent.Endme = true;
        }

        foreach (Konsument konsument in konsumenci)
        {
            konsument.Endme = true;
        }

        Console.WriteLine("Koniec programu.");
        int j = 0;
        foreach (int nn in counter)
        {
            Console.WriteLine($"Producent {j}: {nn}");
            j++;
        }
    }
    
    public static void Zadanie2(){
        string directoryPath="Zad2";

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Podany katalog nie istnieje.");
            return;
        }
        DirectoryMonitor directoryMonitor = new DirectoryMonitor(directoryPath);
        Thread monitorThread = new Thread(directoryMonitor.Monitor);
        monitorThread.Start();
        monitorThread.IsBackground=true;

    
        InputChecker inputChecker = new InputChecker(directoryMonitor);
        Thread inputThread = new Thread(inputChecker.CheckForKey);
        inputThread.Start();
        inputThread.IsBackground=true;

        Console.WriteLine("Naciśnij 'q' aby zakończyć program.");

        monitorThread.Join();
        inputThread.Join();
    
    }
    
    public static void Zadanie3(){
        string directoryPath = "Zad3";

        Console.WriteLine("Wpisz szukaną frazę:");
        string searchText = Console.ReadLine();

        Action<string> foundFileCallback = (filePath) =>
        {
            Console.WriteLine("Znaleziono plik: " + filePath);
        };

        FileSearchThread searchThread = new FileSearchThread(directoryPath, searchText, foundFileCallback);
        searchThread.Start();

        Console.WriteLine("Wyszukiwanie rozpoczęte...");

  
    }
    public static void Zadanie4(){
        Console.WriteLine("Podaj liczbę n:");
        int n = Convert.ToInt32(Console.ReadLine());
        Thread[] threads = new Thread[n];
        AutoResetEvent[] endSignals = new AutoResetEvent[n];

        
        Watek[] workers = new Watek[n];
        for (int i = 0; i < n; i++)
        {
            endSignals[i] = new AutoResetEvent(false);
            workers[i] = new Watek(i, endSignals[i]);
        }
        for (int i = 0; i < n; i++)
        {
            threads[i] = new Thread(workers[i].DoWork);
            threads[i].Start();
        }
        Console.WriteLine("Wszystkie wątki zostały uruchomione.");

        WaitHandle.WaitAll(endSignals);

        Console.WriteLine("Wszystkie wątki zostały zakończone.");

        Console.WriteLine("Program zakończył działanie.");


    }
    public static void Main(string[] args)
    {
        while (true){
            Console.WriteLine("Podaj numer zadania. Wpisz 'x' zeby zakonczyc");
            string numer=Console.ReadLine();
            switch (numer)
            {
                case "1":
                    Zadanie1();
                    break;
                case "2":
                    Zadanie2();
                    break;
                case "3":
                    Zadanie3();
                    break;
                case "4":
                    Zadanie4();
                    break;
                case "x":
                    return;
                default:
                    Console.WriteLine("Wpisano bledny numer");
                    break;
            }
        }
        
    }
}
