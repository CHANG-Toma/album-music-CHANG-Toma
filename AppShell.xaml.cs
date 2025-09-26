namespace Album_music_toma;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Album_music_toma.Views.ArtistDetailPage),
                              typeof(Album_music_toma.Views.ArtistDetailPage));
    }
}
