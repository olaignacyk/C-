using System.Text.RegularExpressions;
class FileSearchThread
{
    private string directoryPath;
    private string searchText;
    private Action<string> foundFileCallback;

    public FileSearchThread(string directoryPath, string searchText, Action<string> foundFileCallback)
    {
        this.directoryPath = directoryPath;
        this.searchText = searchText;
        this.foundFileCallback = foundFileCallback;
    }

    public void Start()
    {
        Thread searchThread = new Thread(SearchFiles);
        searchThread.Start();
    }

    internal void Join()
    {
        throw new NotImplementedException();
    }

    private void SearchFiles()
    {
        SearchInDirectory(directoryPath);
    }

    private void SearchInDirectory(string directory)
    {
        try
        {
            string[] files = Directory.GetFiles(directory, "*.txt");
            foreach (string filePath in files)
            {
                SearchInFile(filePath);
            }

            string[] subdirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subdirectories)
            {
                SearchInDirectory(subdir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas wyszukiwania: " + ex.Message);
        }
    }

    private void SearchInFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, @"\b" + searchText + @"\b", RegexOptions.IgnoreCase))
                {
                    foundFileCallback?.Invoke(filePath);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd podczas czytania pliku " + filePath + ": " + ex.Message);
        }
    }
}