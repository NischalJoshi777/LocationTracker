using SQLite;

namespace LocationTracker
{
    public class LocationService
    {
        private readonly SQLiteConnection _database;

        public LocationService(SQLiteConnection database)
        {
            _database = database;
        }

        public async Task GetLocationAndSaveAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(30)
                });

                if (location != null)
                {
                    _database.Insert(new LocationData
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the location.", ex);            }
        }
    }
}