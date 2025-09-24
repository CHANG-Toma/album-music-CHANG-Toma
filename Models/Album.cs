using System.Collections.Generic;

namespace Album_music___Toma.Models;

public class Album
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string CoverUrl { get; set; } = "";
    public List<Song> Songs { get; set; } = new();
}
