namespace Album_music___Toma;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Album_music___Toma.Views.ArtistDetailPage),
                              typeof(Album_music___Toma.Views.ArtistDetailPage));
    }
}
