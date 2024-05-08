using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Zadanie1{
    public static void Start()
    {
        //ZADANIE 1
        // Uruchomienie serwera w oddzielnym wątku
        Thread serverThread = new Thread(RunServer1);
        serverThread.Start();

        // Poczekaj, aż serwer będzie gotowy
        while (!IsServerRunning)
        {
            Thread.Sleep(100); // Poczekaj 100 ms
        }

        // Uruchomienie klienta
        RunClient1();
    }
        static bool IsServerRunning = false;

    private static void RunServer1()
    {
        // Utwórz serwer nasłuchujący na porcie 11000
        TcpListener server = new TcpListener(IPAddress.Any, 11000);
        server.Start();
        IsServerRunning = true;

        Console.WriteLine("Serwer uruchomiony, oczekiwanie na połączenie...");

        // Zaakceptuj połączenie od klienta
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Połączono z klientem.");

        // Odbierz wiadomość od klienta
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Odebrano wiadomość od klienta: " + receivedMessage);

        // Wypisz odebraną wiadomość na konsoli
        Console.WriteLine("Odebrana wiadomość: " + receivedMessage);

        // Ogranicz długość wiadomości do 1024 bajtów
        if (receivedMessage.Length > 1024)
        {
            receivedMessage = receivedMessage.Substring(0, 1024);
        }

        // Wyślij wiadomość zwrotną do klienta
        string responseMessage = "odczytalem: " + receivedMessage;
        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        stream.Write(responseBuffer, 0, responseBuffer.Length);

        // Zakończ połączenie
        client.Close();
        server.Stop();

        Console.WriteLine("Serwer zakończył działanie.");
    }

    private static void RunClient1()
    {
        // Utwórz połączenie z serwerem na porcie 11000
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Adres IP serwera
        IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {
            socket.Connect(remoteEndPoint);
            Console.WriteLine("Połączono z serwerem.");

            // Wprowadź wiadomość do wysłania
            Console.Write("Wpisz wiadomość: ");
            string message = Console.ReadLine();

            // Ogranicz długość wiadomości do 1024 bajtów
            if (message.Length > 1024)
            {
                message = message.Substring(0, 1024);
            }

            // Wyślij wiadomość do serwera
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            socket.Send(messageBytes);

            // Odbierz odpowiedź od serwera
            byte[] buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            string responseMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Odebrano odpowiedź od serwera: " + responseMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas połączenia z serwerem: " + ex.Message);
        }
        finally
        {
            // Zakończ połączenie
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}

class Zadanie2{
    public static void Start(){

        var serverThread = new System.Threading.Thread(RunServer);
        serverThread.Start();

        // Poczekaj, aż serwer będzie gotowy
        System.Threading.Thread.Sleep(100); // Oczekiwanie 100 ms

        // Uruchomienie klienta
        RunClient();
    }

    static void RunServer()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 11000);
        server.Start();
        Console.WriteLine("Serwer uruchomiony, oczekiwanie na połączenie...");

        // Zaakceptuj połączenie od klienta
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Połączono z klientem.");

        // Odbierz rozmiar wiadomości
        byte[] sizeBuffer = new byte[4];
        client.GetStream().Read(sizeBuffer, 0, sizeBuffer.Length);
        int messageSize = BitConverter.ToInt32(sizeBuffer, 0);
        Console.WriteLine("Odebrano rozmiar wiadomości: " + messageSize);

        // Odbierz właściwą wiadomość
        byte[] buffer = new byte[messageSize];
        client.GetStream().Read(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.UTF8.GetString(buffer);
        Console.WriteLine("Odebrano wiadomość: " + receivedMessage);

        // Wysłanie potwierdzenia do klienta
        byte[] confirmation = Encoding.UTF8.GetBytes("Wiadomość odebrana");
        client.GetStream().Write(confirmation, 0, confirmation.Length);

        // Zakończ połączenie
        client.Close();
        server.Stop();
    }

    static void RunClient()
    {
        // Utwórz połączenie z serwerem na porcie 11000
        using (TcpClient client = new TcpClient("127.0.0.1", 11000))
        {
            NetworkStream stream = client.GetStream();

            // Wprowadź wiadomość do wysłania
            Console.Write("Wpisz wiadomość: ");
            string message = Console.ReadLine();

            // Konwertuj wiadomość na tablicę bajtów
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // Wysłanie rozmiaru wiadomości jako 4-bajtowej wartości
            byte[] sizeBytes = BitConverter.GetBytes(messageBytes.Length);
            stream.Write(sizeBytes, 0, sizeBytes.Length);

            // Wysłanie właściwej wiadomości
            stream.Write(messageBytes, 0, messageBytes.Length);

            // Odbierz potwierdzenie od serwera
            byte[] confirmation = new byte[64];
            int bytesRead = stream.Read(confirmation, 0, confirmation.Length);
            string responseMessage = Encoding.UTF8.GetString(confirmation, 0, bytesRead);
            Console.WriteLine("Odebrano potwierdzenie od serwera: " + responseMessage);
        }
    }
}
class Zadanie3
{
    private static string myDir = Directory.GetCurrentDirectory();

    public static void Start()
    {
        var serverThread = new System.Threading.Thread(RunServer);
        serverThread.Start();


        System.Threading.Thread.Sleep(100);

        RunClient();
    }

    static void RunServer()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 11000);
        server.Start();
        Console.WriteLine("Serwer uruchomiony, oczekiwanie na połączenie...");

        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Połączono z klientem.");

        NetworkStream stream = client.GetStream();

        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        while (true)
        {
            try
            {
                string requestMessage = reader.ReadLine();

                string responseMessage = HandleMessage(requestMessage);

                writer.Write(responseMessage);

                if (requestMessage == "!end")
                    break;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas obsługi wiadomości: " + ex.Message);
                break;
            }
        }

        client.Close();
        server.Stop();
    }

    static void RunClient()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 11000))
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            Console.WriteLine("Połączono z serwerem.");

            while (true)
            {
                try
                {
                    Console.Write("Wpisz wiadomość: ");
                    string message = Console.ReadLine();

                    writer.WriteLine(message);

                    if (message == "!end")
                        break;

                    string response = reader.ReadLine();
                    Console.WriteLine("Odpowiedź od serwera: " + response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd podczas komunikacji z serwerem: " + ex.Message);
                    break;
                }
            }
        }
    }

    static string HandleMessage(string message)
    {
        if (message == "!end")
        {
            return "!end";
        }
        else if (message == "list")
        {
            string[] entries = Directory.GetFileSystemEntries(myDir);
            return string.Join(Environment.NewLine, entries);
        }
        else if (message.StartsWith("in "))
        {
            string subDir = message.Substring(3);
            string newPath;

            if (subDir == "..")
            {
                newPath = Directory.GetParent(myDir).FullName;
            }
            else
            {
                string newDir = Path.Combine(myDir, subDir);
                if (Directory.Exists(newDir))
                {
                    newPath = newDir;
                }
                else
                {
                    return "Katalog nie istnieje";
                }
            }

            myDir = newPath;

            string[] entries = Directory.GetFileSystemEntries(myDir);
            return string.Join(Environment.NewLine, entries);
        }
        else
        {
            return "Nieznane polecenie";
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true){
        Console.WriteLine("Wybierz numer zadania, x zeby zakonczyc");
        string numer=Console.ReadLine();
        switch (numer)
            {
                case "1":
                    Zadanie1.Start();
                    break;
                case "2":
                    Zadanie2.Start();
                    break;
                case "3":
                    Zadanie3.Start();
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
