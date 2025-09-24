using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Album_music___Toma.Models;

namespace Album_music___Toma.ViewModels;

public class ArtistsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Artist> Artists { get; } = new();
    public ObservableCollection<Artist> FilteredArtists { get; } = new();

    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set { if (Set(ref _searchText, value)) ApplyFilter(); }
    }

    private string _genreFilter = "All";
    public string GenreFilter
    {
        get => _genreFilter;
        set { if (Set(ref _genreFilter, value)) ApplyFilter(); }
    }

    public string[] Genres { get; } = new[] { "All", "Pop", "Rock", "Rap", "Classique" };

    public ICommand SelectArtistCommand { get; }

    // Constructeur
    public ArtistsViewModel()
    {
        LoadSeed(); // Charger les données initiales
        ApplyFilter(); // Appliquer le filtre initial

        // Commande pour sélectionner un artiste
        SelectArtistCommand = new Command<Artist>(async artist =>
        {
            if (artist is null) return;
            await Shell.Current.GoToAsync(nameof(Album_music___Toma.Views.ArtistDetailPage),
                new Dictionary<string, object> { { "Artist", artist } });
        });
    }

    // Méthode pour charger les données pour tester l'affichage
    void LoadSeed()
    {
        Artists.Add(new Artist
        {
            Name = "Stromae",
            Genre = "Pop",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/9/9e/Stromae_2011.jpg",
            Albums = {
                new Album {
                    Title = "Racine Carree", Year = 2013,
                    CoverUrl = "https://i.scdn.co/image/ab67616d0000b273f0e2e8f0f9f2f3a2f3a2f3a2",
                    Songs = {
                        new Song { Title = "Formidable", YoutubeUrl = "https://www.youtube.com/watch?v=S_xH7noaqTA" },
                        new Song { Title = "Papaoutai",  YoutubeUrl = "https://www.youtube.com/watch?v=oiKj0Z_Xnjc" }
                    }
                }
            }
        });

        Artists.Add(new Artist
        {
            Name = "Celine Dion",
            Genre = "Pop",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/2/2f/Celine_Dion_2012.jpg"
        });

        Artists.Add(new Artist
        {
            Name = "Ed Sheeran",
            Genre = "Pop",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/4/45/Ed_Sheeran_2013.jpg"
        });

        Artists.Add(new Artist
        {
            Name = "AC/DC",
            Genre = "Rock",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7f/ACDC_In_Tacoma_2009.jpg"
        });
    }

    // Méthode pour appliquer les filtres de recherche et de genre
    void ApplyFilter()
    {
        var q = Artists.AsEnumerable();

        // Filtrer par texte
        if (!string.IsNullOrWhiteSpace(SearchText))
            q = q.Where(a => a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        // Filtrer par genre
        if (!string.Equals(GenreFilter, "All", StringComparison.OrdinalIgnoreCase))
            q = q.Where(a => string.Equals(a.Genre, GenreFilter, StringComparison.OrdinalIgnoreCase));

        ReplaceCollection(FilteredArtists, q);
    }

    // Méthode utilitaire pour remplacer le contenu d'une ObservableCollection
    static void ReplaceCollection<T>(ObservableCollection<T> target, System.Collections.Generic.IEnumerable<T> items)
    {
        // Remplacer le contenu de la collection cible par les nouveaux éléments
        target.Clear();
        foreach (var it in items) target.Add(it);
    }

    // Implémentation de INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    bool Set<T>(ref T storage, T value, [CallerMemberName] string? name = null)
    {
        // Si la valeur est la même, ne rien faire
        if (Equals(storage, value)) return false;
        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        return true;
    }
}
