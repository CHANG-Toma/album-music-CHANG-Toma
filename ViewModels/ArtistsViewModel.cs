using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Album_music_toma.Models;
using Microsoft.Maui.Controls;

// ViewModel pour la gestion des artistes et des filtres
namespace Album_music_toma.ViewModels
{
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

        // Liste des genres disponibles
        public string[] Genres { get; } = new[] { "All", "Pop", "Rock", "Rap", "Classique" };

        public ICommand SelectArtistCommand { get; }

        // Constructeur
        public ArtistsViewModel()
        {
            LoadSeed();
            ApplyFilter();

            SelectArtistCommand = new Command<Artist>(async artist =>
            {
                if (artist is null) return;
                await Shell.Current.GoToAsync(nameof(Album_music_toma.Views.ArtistDetailPage),
                    new Dictionary<string, object> { { "Artist", artist } });
            });
        }

        void LoadSeed()
        {
            Artists.Add(new Artist
            {
                Name = "Stromae",
                Genre = "Pop",
                PhotoUrl = "stromae.svg",
                Bio = "Paul Van Haver, connu sous le nom de Stromae, est un chanteur, rappeur et compositeur belge. Il est reconnu pour ses textes profonds et sa musique électronique unique, mélangeant hip-hop et musique électronique. Ses chansons abordent souvent des thèmes sociaux et personnels avec une approche artistique innovante.",
                Albums = new List<Album> {
                    new Album { 
                        Title = "Racine Carrée", 
                        Year = 2013,
                        CoverUrl = "racine_carree.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Papaoutai", YoutubeUrl = "https://www.youtube.com/watch?v=oiKj0Z_Xnjc" },
                            new Song { Title = "Formidable", YoutubeUrl = "https://www.youtube.com/watch?v=Jc7QdD3Yq2s" },
                            new Song { Title = "Tous les mêmes", YoutubeUrl = "https://www.youtube.com/watch?v=2s5lJ5NqyVY" }
                        }
                    },
                    new Album { 
                        Title = "Multitude", 
                        Year = 2022,
                        CoverUrl = "racine_carree.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Santé", YoutubeUrl = "https://www.youtube.com/watch?v=5qap5aO4i9A" },
                            new Song { Title = "L'enfer", YoutubeUrl = "https://www.youtube.com/watch?v=5qap5aO4i9A" }
                        }
                    }
                }
            });

            Artists.Add(new Artist
            {
                Name = "Céline Dion",
                Genre = "Pop",
                PhotoUrl = "celine_dion.svg",
                Bio = "Céline Dion est une chanteuse québécoise de renommée internationale. Avec sa voix puissante et émouvante, elle a vendu plus de 200 millions d'albums dans le monde. Elle est particulièrement connue pour ses ballades romantiques et sa performance emblématique du thème du film Titanic, 'My Heart Will Go On'.",
                Albums = new List<Album> {
                    new Album { 
                        Title = "Falling into You", 
                        Year = 1996,
                        CoverUrl = "falling_into_you.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Because You Loved Me", YoutubeUrl = "https://www.youtube.com/watch?v=YQHsXMglC9A" },
                            new Song { Title = "It's All Coming Back to Me Now", YoutubeUrl = "https://www.youtube.com/watch?v=0i7O9P4X3h0" }
                        }
                    }
                }
            });

            Artists.Add(new Artist
            {
                Name = "Ed Sheeran",
                Genre = "Pop",
                PhotoUrl = "ed_sheeran.svg",
                Bio = "Ed Sheeran est un auteur-compositeur-interprète britannique connu pour sa musique acoustique et ses mélodies accrocheuses. Avec sa guitare et sa voix distinctive, il a conquis le monde avec des hits comme 'Shape of You' et 'Perfect'. Il est reconnu pour son talent de parolier et sa capacité à créer des chansons intemporelles.",
                Albums = new List<Album> {
                    new Album { 
                        Title = "÷ (Divide)", 
                        Year = 2017,
                        CoverUrl = "divide.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Shape of You", YoutubeUrl = "https://www.youtube.com/watch?v=JGwWNGJdvx8" },
                            new Song { Title = "Castle on the Hill", YoutubeUrl = "https://www.youtube.com/watch?v=K0ibBPhiaG0" }
                        }
                    }
                }
            });

            Artists.Add(new Artist
            {
                Name = "AC/DC",
                Genre = "Rock",
                PhotoUrl = "acdc.svg",
                Bio = "AC/DC est un groupe de rock australien formé en 1973 par les frères Malcolm et Angus Young. Le groupe est célèbre pour ses riffs de guitare électrique puissants et ses chansons énergiques. Avec des hits légendaires comme 'Highway to Hell' et 'Back in Black', AC/DC est devenu l'un des groupes de rock les plus influents de l'histoire.",
                Albums = new List<Album> {
                    new Album { 
                        Title = "Back in Black", 
                        Year = 1980,
                        CoverUrl = "back_in_black.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Back in Black", YoutubeUrl = "https://www.youtube.com/watch?v=pAgnJDJ4q1M" },
                            new Song { Title = "Thunderstruck", YoutubeUrl = "https://www.youtube.com/watch?v=v2AC41dglnM" }
                        }
                    }
                }
            });

            Artists.Add(new Artist
            {
                Name = "Mozart",
                Genre = "Classique",
                PhotoUrl = "mozart.svg",
                Bio = "Wolfgang Amadeus Mozart (1756-1791) était un compositeur autrichien de la période classique. Considéré comme l'un des plus grands génies musicaux de l'histoire, Mozart a composé plus de 600 œuvres incluant des symphonies, des concertos, des opéras et de la musique de chambre. Ses compositions comme la Symphonie n°40 et l'opéra 'La Flûte enchantée' restent des chefs-d'œuvre intemporels.",
                Albums = new List<Album> {
                    new Album { 
                        Title = "Symphonie n°40", 
                        Year = 1788,
                        CoverUrl = "symphonie_40.svg",
                        Songs = new List<Song> {
                            new Song { Title = "Symphonie n°40 - 1er mouvement", YoutubeUrl = "https://www.youtube.com/watch?v=JTc1mDieQI8" }
                        }
                    }
                }
            });
        }

        // Applique les filtres de recherche et de genre
        void ApplyFilter()
        {
            var q = Artists.AsEnumerable();

            // si non vide, on filtre sur le nom
            if (!string.IsNullOrWhiteSpace(SearchText))
                q = q.Where(a => a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            // si différent de "All", on filtre sur le genre
            if (!string.Equals(GenreFilter, "All", StringComparison.OrdinalIgnoreCase))
                q = q.Where(a => string.Equals(a.Genre, GenreFilter, StringComparison.OrdinalIgnoreCase));

            // on met à jour la collection filtrée
            ReplaceCollection(FilteredArtists, q);
        }

        // Remplace le contenu d'une ObservableCollection
        static void ReplaceCollection<T>(ObservableCollection<T> target, System.Collections.Generic.IEnumerable<T> items)
        {
            target.Clear();
            foreach (var it in items) target.Add(it);
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        bool Set<T>(ref T storage, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }
    }
}
