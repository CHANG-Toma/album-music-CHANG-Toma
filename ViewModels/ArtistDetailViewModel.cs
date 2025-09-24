using System.Collections.ObjectModel;
using System.Windows.Input;
using Album_music___Toma.Models;

namespace Album_music___Toma.ViewModels;

public class ArtistDetailViewModel : IQueryAttributable
{
    public Artist Artist { get; private set; } = new();
    public ObservableCollection<Album> Albums { get; } = new();

    public ICommand OpenSongCommand { get; }

    public ArtistDetailViewModel()
    {
        OpenSongCommand = new Command<string>(async url =>
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            await Launcher.OpenAsync(url);
        });
    }

    // Permet de recevoir les paramètres de la navigation et de mettre à jour l'interface concernant les details de l'artiste
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Artist", out var obj) && obj is Artist a)
        {
            Artist = a;
            Albums.Clear();
            foreach (var al in a.Albums) Albums.Add(al);
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Artist)));
        }
    }

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
}
