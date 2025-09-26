using System.Collections.Generic;

namespace Album_music___Toma.Models;

// Classe Artist
public class Artist
{
    public string Name { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Bio { get; set; } = "";
    public List<Album> Albums { get; set; } = new();
}
