using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Album_music_toma.Models;
using Microsoft.Maui.Controls;

namespace Album_music_toma.ViewModels
{
    public class ArtistDetailViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private Artist _artist = new();
        public Artist Artist 
        { 
            get => _artist; 
            private set 
            { 
                if (Set(ref _artist, value))
                {
                    Albums.Clear();
                    if (value?.Albums != null)
                    {
                        foreach (var album in value.Albums) 
                            Albums.Add(album);
                    }
                }
            } 
        }
        
        public ObservableCollection<Album> Albums { get; } = new();
        public ICommand ViewSongsCommand { get; }

        // Constructeur
        public ArtistDetailViewModel()
        {
            ViewSongsCommand = new Command<Album>(async album =>
            {
                System.Diagnostics.Debug.WriteLine("=== ViewSongsCommand DÉCLENCHÉ ===");
                System.Diagnostics.Debug.WriteLine($"Album reçu: {(album?.Title ?? "NULL")}");
                System.Diagnostics.Debug.WriteLine($"Artist.Name: {Artist?.Name ?? "NULL"}");
                
                try
                {
                    // Navigation simple d'abord
                    System.Diagnostics.Debug.WriteLine("Tentative de navigation vers 'songs'...");
                    await Shell.Current.GoToAsync("songs");
                    System.Diagnostics.Debug.WriteLine("✅ Navigation réussie vers songs");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Erreur navigation: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            });
        }

        // Reçoit l'objet depuis Shell
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Artist", out var obj) && obj is Artist a)
            {
                Artist = a;
                // Notifier explicitement le changement de l'artiste
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Artist)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
