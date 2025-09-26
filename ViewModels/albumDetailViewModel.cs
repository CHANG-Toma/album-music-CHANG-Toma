// viewmodels/albumDetailViewModel.cs
using Album_music___Toma.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Album_music___Toma.ViewModels;

public class AlbumDetailViewModel : IQueryAttributable
{
    public Album Album { get; private set; } = new();
    public ObservableCollection<Song> Songs { get; } = new();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Album", out var obj) && obj is Album a)
        {
            Album = a;
            Songs.Clear();
            foreach (var s in a.Songs) Songs.Add(s);
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Album)));
        }
    }

    public ICommand OpenSongCommand { get; }

    public AlbumDetailViewModel()
    {
        OpenSongCommand = new Command<string>(async url =>
        {
            if (!string.IsNullOrWhiteSpace(url))
                await Launcher.OpenAsync(url);
        });
    }

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
}
