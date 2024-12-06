
using SQLite;

namespace LocationTracker
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection _database;
        private CancellationTokenSource _cancellationTokenSource;
        private LocationService _locationService;
        private MapService _mapService;

        public MainPage()
        {
            InitializeComponent();
            InitializeDatabase();
            _locationService = new LocationService(_database);
            _mapService = new MapService(map, _database);
            Title = "MainView";
        }

    /// <summary>
    /// INITIALIZES SQLITE DATABASE
    /// </summary>
        private void InitializeDatabase()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Locations.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<LocationData>();
            Console.WriteLine($"Database Path: {dbPath}");
        }

        private async Task UpdateLocationAndMap()
        {
            try { 
                await _locationService.GetLocationAndSaveAsync();
                _mapService.UpdateMapWithHeatmap();
            } catch (Exception ex){
                await DisplayAlert("Error", $"Failed to get location: {ex.Message}", "OK");
            }
           
        }

        private void StartLocationTracking()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await UpdateLocationAndMap();
                    await Task.Delay(30000);
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartLocationTracking();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _cancellationTokenSource?.Cancel();
        }
    }
}
