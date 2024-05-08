class DirectoryMonitor
{
    private string directoryPath;
    private bool running = true;

    public DirectoryMonitor(string directoryPath)
    {
        this.directoryPath = directoryPath;
    }

    public void Monitor()
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = directoryPath;
        watcher.NotifyFilter = NotifyFilters.FileName;

        // Dodanie obsługi zdarzeń
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;

        // Rozpoczęcie monitorowania
        watcher.EnableRaisingEvents = true;

        // Pętla monitorująca będzie działać dopóki program jest uruchomiony
        while (running)
        {
            Thread.Sleep(1000); // Odczekanie przed sprawdzeniem kolejnej zmiany
        }

        watcher.EnableRaisingEvents = false; // Wyłączenie monitorowania po zakończeniu programu
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Dodano plik: {e.Name}");
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Usunięto plik: {e.Name}");
    }

    public void StopMonitoring()
    {
        running = false;
    }
}

class InputChecker
{
    private bool running = true;
    private DirectoryMonitor directoryMonitor;

    public InputChecker(DirectoryMonitor directoryMonitor)
    {
        this.directoryMonitor = directoryMonitor;
    }

    public void CheckForKey()
    {
        while (running)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.KeyChar == 'q')
                {
                    directoryMonitor.StopMonitoring(); // Zatrzymanie monitorowania katalogu
                    running = false;
                    break;
                }
            }
            Thread.Sleep(100); // Poczekaj chwilę przed kolejnym sprawdzeniem klawisza
        }
    }
}