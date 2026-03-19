using System;
using System.Net;
using System.Text.Json;

List<string> getFilmography(string searchActor, string url){

    List<string> filmography = new List<string>();

    string json;
    using (var wc = new WebClient())
    {
        json = wc.DownloadString(url);
    }

    using var doc = JsonDocument.Parse(json);
    foreach (var movie in doc.RootElement.EnumerateArray())
    {
        if (movie.TryGetProperty("cast", out var cast))
        {
            foreach (var actor in cast.EnumerateArray())
            {
                if (actor.GetString().ToString().ToLowerInvariant()==searchActor.ToLowerInvariant())
                {
                    string title = movie.GetProperty("title").GetString();
                    int year = movie.GetProperty("year").GetInt32();

                    filmography.Add($"{title} ({year})");
                    break;
                }
            }
        }
    }
    if (filmography.Count() == 0) return new List<string>{"Actor Not Found"};
    return filmography;
}

Console.WriteLine("Enter Actor:");
string searchActor = Console.ReadLine();

string url = "https://raw.githubusercontent.com/prust/wikipedia-movie-data/master/movies.json";
List<string> filmography = getFilmography(searchActor, url);
foreach(string film in filmography)
    Console.WriteLine(film);

