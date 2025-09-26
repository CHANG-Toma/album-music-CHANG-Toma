using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Album_music_toma.Models;
using Microsoft.Maui.Controls;

namespace Album_music_toma.ViewModels
{
    public class SongsViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        private Album _album = new();
        public Album Album 
        { 
            get => _album; 
            private set 
            { 
                if (Set(ref _album, value))
                {
                    Songs.Clear();
                    if (value?.Songs != null)
                    {
                        foreach (var song in value.Songs) 
                            Songs.Add(song);
                    }
                }
            } 
        }
        
        private string _artistName = "";
        public string ArtistName 
        { 
            get => _artistName; 
            private set => Set(ref _artistName, value); 
        }
        
        private string _videoUrl = "";
        public string VideoUrl 
        { 
            get => _videoUrl; 
            private set => Set(ref _videoUrl, value); 
        }
        
        private string _currentSongTitle = "";
        public string CurrentSongTitle 
        { 
            get => _currentSongTitle; 
            private set => Set(ref _currentSongTitle, value); 
        }
        
        private bool _isVideoVisible = false;
        public bool IsVideoVisible 
        { 
            get => _isVideoVisible; 
            private set => Set(ref _isVideoVisible, value); 
        }
        
        public ObservableCollection<Song> Songs { get; } = new();
        public ICommand PlaySongCommand { get; }
        public ICommand CloseVideoCommand { get; }

        // Constructeur
        public SongsViewModel()
        {
            PlaySongCommand = new Command<string>(async url =>
            {
                if (string.IsNullOrWhiteSpace(url)) return;
                
                try
                {
                    // Convertir l'URL YouTube en URL d'embed et afficher la vidéo
                    var embedUrl = ConvertToEmbedUrl(url);
                    VideoUrl = embedUrl;
                    IsVideoVisible = true;
                    
                    // Trouver le titre de la chanson
                    var song = Songs.FirstOrDefault(s => s.YoutubeUrl == url);
                    if (song != null)
                    {
                        CurrentSongTitle = $"🎵 {song.Title}";
                    }
                    
                    // Attendre un peu pour que le WebView se charge
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    // En cas d'erreur, afficher un message d'erreur
                    CurrentSongTitle = "❌ Erreur de chargement";
                    System.Diagnostics.Debug.WriteLine($"Erreur lecture vidéo: {ex.Message}");
                }
            });

            CloseVideoCommand = new Command(() =>
            {
                IsVideoVisible = false;
                VideoUrl = "";
                CurrentSongTitle = "";
            });
        }

        // Reçoit l'objet depuis Shell
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue("Album", out var albumObj) && albumObj is Album album)
                {
                    Album = album;
                    System.Diagnostics.Debug.WriteLine($"Album reçu: {album.Title}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Aucun album reçu - utilisation d'un album par défaut");
                    // Créer un album par défaut pour éviter les erreurs
                    Album = new Album
                    {
                        Title = "Sélection d'albums",
                        Year = 2024,
                        CoverUrl = "dotnet_bot.png",
                        Songs = new List<Song>
                        {
                            new Song { Title = "Chanson 1", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" },
                            new Song { Title = "Chanson 2", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" },
                            new Song { Title = "Chanson 3", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" }
                        }
                    };
                }
                
                if (query.TryGetValue("ArtistName", out var artistObj) && artistObj is string artistName)
                {
                    ArtistName = artistName;
                    System.Diagnostics.Debug.WriteLine($"Nom artiste reçu: {artistName}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Aucun nom d'artiste reçu - utilisation d'un nom par défaut");
                    ArtistName = "Artiste";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur ApplyQueryAttributes: {ex.Message}");
                // En cas d'erreur, créer des données par défaut
                Album = new Album
                {
                    Title = "Album par défaut",
                    Year = 2024,
                    CoverUrl = "dotnet_bot.png",
                    Songs = new List<Song>
                    {
                        new Song { Title = "Chanson par défaut", YoutubeUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ" }
                    }
                };
                ArtistName = "Artiste par défaut";
            }
        }

        private string ConvertToEmbedUrl(string youtubeUrl)
        {
            try
            {
                // Extraire l'ID de la vidéo YouTube
                var videoId = ExtractVideoId(youtubeUrl);
                if (!string.IsNullOrEmpty(videoId))
                {
                    // Créer l'URL d'embed pour lecture directe avec plus de paramètres
                    return $"https://www.youtube.com/embed/{videoId}?autoplay=1&rel=0&modestbranding=1&showinfo=0&controls=1&fs=1&cc_load_policy=0&iv_load_policy=3&start=0&end=0&loop=0&playlist={videoId}";
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, retourner l'URL originale
                System.Diagnostics.Debug.WriteLine($"Erreur conversion YouTube: {ex.Message}");
            }
            
            return youtubeUrl;
        }

        private string ExtractVideoId(string youtubeUrl)
        {
            try
            {
                var uri = new Uri(youtubeUrl);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                
                // Essayer de récupérer l'ID depuis le paramètre 'v'
                var videoId = query["v"];
                if (!string.IsNullOrEmpty(videoId))
                {
                    return videoId;
                }
                
                // Si c'est un format court (youtu.be/ID)
                if (uri.Host.Contains("youtu.be"))
                {
                    return uri.Segments.Last().Trim('/');
                }
            }
            catch
            {
                // En cas d'erreur, essayer d'extraire manuellement
                var match = System.Text.RegularExpressions.Regex.Match(youtubeUrl, @"(?:youtube\.com\/watch\?v=|youtu\.be\/)([^&\n?#]+)");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            
            return "";
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
