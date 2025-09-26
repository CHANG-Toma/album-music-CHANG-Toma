namespace Album_music_toma.Views;

public partial class ArtistDetailPage : ContentPage
{
    public ArtistDetailPage() { InitializeComponent(); }
    
    private async void OnViewAllSongsClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("🎵 OnViewAllSongsClicked DÉCLENCHÉ !");
        
        // Test simple : afficher une alerte pour confirmer que le clic fonctionne
        await DisplayAlert("Test", "Le bouton fonctionne !", "OK");
        
        try
        {
            // Navigation simple vers la page songs
            System.Diagnostics.Debug.WriteLine("Navigation vers 'songs'...");
            await Shell.Current.GoToAsync("songs");
            System.Diagnostics.Debug.WriteLine("✅ Navigation réussie vers songs");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Erreur navigation: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            await DisplayAlert("Erreur", $"Erreur de navigation: {ex.Message}", "OK");
        }
    }
}
