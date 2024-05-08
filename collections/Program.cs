using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Text;


public class Tweet
{
    public string Text { get; set; }
    public string UserName { get; set; }
    public string CreatedAt { get; set; }
    public string FirstLinkUrl {get;set;}
    public string TweetEmbedCode {get;set;}
    public string LinkToTweet {get;set;}
}

public class Program
{
    public static void Main()
    {
        // 1. Wczytaj dane z pliku JSON
        List<Tweet> tweets = LoadTweetsFromJson("data.json");
        Console.WriteLine("Dane zostały wczytane z pliku JSON.");
        Console.WriteLine("____________________________________________");

        for (int i = 0; i < Math.Min(10, tweets.Count); i++)
        {
            Console.WriteLine("Username: "+tweets[i].UserName+"\n"+ "Text: "+tweets[i].Text +"\n"+"Data: " + tweets[i].CreatedAt);
        }
        Console.WriteLine("____________________________________________");

        // 2. Zapisz dane do XML
        SaveTweetsToXml(tweets, "tweets.xml");
        Console.WriteLine("Dane zostały zapisane do pliku XML.");
        Console.WriteLine("____________________________________________");

        // Wczytaj dane z pliku XML
        List<Tweet> loadedTweets = LoadTweetsFromXml("tweets.xml");
        Console.WriteLine("Dane zostały wczytane z pliku XML.");
        Console.WriteLine("____________________________________________");

        // 3. Sortuj tweety według nazwy użytkownika
        List<Tweet> sortedByUsername = tweets.OrderBy(tweet => tweet.UserName).ToList();
        Console.WriteLine("Tweety zostały posortowane według nazwy użytkownika.");
        PrintFirstTen(sortedByUsername,false);
        Console.WriteLine("____________________________________________");

        // Sortuj użytkowników według daty utworzenia tweeta
        List<Tweet> sortedUsersByDate = tweets.OrderBy(tweet =>
        {
            DateTime date;
            DateTime.TryParse(tweet.CreatedAt.Trim(), out date);
            return date;
        }).ToList();
        Console.WriteLine("Użytkownicy zostały posortowani według daty utworzenia tweeta.");
        PrintFirstTen(sortedUsersByDate,true);

        Console.WriteLine("____________________________________________");

        // 4. Znajdź najnowszy i najstarszy tweet
        Tweet newestTweet = sortedUsersByDate.Last();
        Tweet oldestTweet = sortedUsersByDate.First();
        Console.WriteLine("Znaleziono najnowszy i najstarszy tweet.");
        Console.WriteLine("Najstarszy tweet: " + oldestTweet.CreatedAt);
        Console.WriteLine("Najnowszy tweet: " + newestTweet.CreatedAt);
        Console.WriteLine("____________________________________________");

        Dictionary<string, List<Tweet>> tweetsByUser = tweets.GroupBy(tweet => tweet.UserName)
            .ToDictionary(group => group.Key, group => group.ToList());
        Console.WriteLine("Utworzono słownik tweetów pogrupowanych według użytkownika:");
        PrintFirstFiveDictionary(tweetsByUser);
        Console.WriteLine("____________________________________________");

        // 6. Oblicz częstotliwość występowania słów
        Dictionary<string, int> wordFrequency = CalculateWordFrequency(tweets);
        Console.WriteLine("Obliczono częstotliwość występowania słów:");
        PrintFirstFiveWordFrequency(wordFrequency);
        Console.WriteLine("____________________________________________");

        // 7. Wypisz 10 najczęściej występujących wyrazów o długości co najmniej 5 liter
        var top10Words = wordFrequency.Where(w => w.Key.Length >= 5)
            .OrderByDescending(w => w.Value)
            .Take(10);
        Console.WriteLine("Wypisano 10 najczęściej występujących wyrazów o długości co najmniej 5 liter:");
        Console.WriteLine("____________________________________________");
        foreach (var word in top10Words)
        {
            Console.WriteLine($"{word.Key}: {word.Value} razy");
        }
        Console.WriteLine("____________________________________________");

        // 8. Oblicz IDF dla wszystkich slow w tweetach
        CountIDF(tweets);
        Console.WriteLine("____________________________________________");
    
    }

    static List<Tweet> LoadTweetsFromJson(string filePath)
    {
        Console.WriteLine("Wczytywanie danych z pliku JSON: " + filePath);
        List<Tweet> tweets = new List<Tweet>();
        foreach (var line in File.ReadLines(filePath))
        {
            var tweet = JsonSerializer.Deserialize<Tweet>(line);
            tweets.Add(tweet);
        }
        return tweets;
    }

    static void SaveTweetsToXml(List<Tweet> tweets, string filePath)
    {
        Console.WriteLine("Zapisywanie danych do pliku XML: " + filePath);
        XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, tweets);
        }
    }

    static List<Tweet> LoadTweetsFromXml(string filePath)
    {
        Console.WriteLine("Wczytywanie danych z pliku XML: " + filePath);
        XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
        using (StreamReader reader = new StreamReader(filePath))
        {
            return (List<Tweet>)serializer.Deserialize(reader);
        }
    }

    static Dictionary<string, int> CalculateWordFrequency(List<Tweet> tweets)
    {
        Console.WriteLine("Obliczanie częstotliwości występowania słów.");
        Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
        foreach (var tweet in tweets)
        {
            string[] words = Regex.Split(tweet.Text.ToLower(), @"[^a-z0-9]+");
            foreach (var word in words)
            {
                if (word.Length >= 5)
                {
                    if (wordFrequency.ContainsKey(word))
                    {
                        wordFrequency[word]++;
                    }
                    else
                    {
                        wordFrequency[word] = 1;
                    }
                }
            }
        }
        return wordFrequency;
    }
    static void CountIDF(List<Tweet> tweets)
    {
        Dictionary<string, int> wordFrequency= new Dictionary<string, int>();
        int wordCount=tweets.Count;
               foreach (var tweet in tweets)
        {
            string[] words = Regex.Split(tweet.Text.ToLower(), @"[^a-z0-9]+");
            foreach (var word in words.Distinct())
            {

                if (wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency[word] = 1;
                }
                }
            }
        Dictionary<string, double> counterIDF = new Dictionary<string, double>();
        foreach (var pair in wordFrequency)
        {
            counterIDF[pair.Key] = Math.Log((double)wordCount / pair.Value);
        }
        var sortedIDF = counterIDF.OrderByDescending(pair => pair.Value);

        Console.WriteLine("10 najwyższych wartości IDF:");

        int count = 0;
        foreach (var pair in sortedIDF)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
            count++;
            if (count == 10) break;
            }
    
}
    static void PrintFirstTen(List<Tweet> tweets,bool czyData)
    {
        Console.WriteLine("Pierwsze 10 tweetów po posortowaniu:");
        for (int i = 0; i < Math.Min(10, tweets.Count); i++)
        {
            if (czyData){
                Console.WriteLine($"{i + 1}. {tweets[i].CreatedAt}");
            }
            else{
                Console.WriteLine($"{i + 1}. {tweets[i].UserName}");
            }
        }
    }
    static void PrintFirstFiveDictionary(Dictionary<string, List<Tweet>> dictionary)
        {
            int count = 0;
            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key}:");
                foreach (var tweet in item.Value)
                {
                    Console.WriteLine($"- {tweet.Text}");
                    count++;
                    if (count >= 5)
                        return;
                }
            }
        }

    static void PrintFirstFiveWordFrequency(Dictionary<string, int> dictionary)
    {
        int count = 0;
        foreach (var item in dictionary)
        {
            Console.WriteLine($"{item.Key}: {item.Value} razy");
            count++;
            if (count >= 5)
                return;
        }
    }
}
