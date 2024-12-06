using SQLite;

namespace LocationTracker
{
    public class LocationService
    {
        private readonly SQLiteConnection _database;

         /// <summary>
        /// Initializes the service with a database connection.
        /// </summary>
        /// <param name="database">SQLite database connection.</param>

        public LocationService(SQLiteConnection database)
        {
            _database = database;
        }

        /// <summary>
        /// Retrieves the user's current or last known location and saves it to the database.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
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