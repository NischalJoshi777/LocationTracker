using Microsoft.Maui.Maps;
using SQLite;


namespace LocationTracker
{

    // <summary>
    /// Service class responsible for managing and updating the map with various features like heatmaps.
    /// </summary>
    public class MapService
    {
        private readonly Microsoft.Maui.Controls.Maps.Map _map;
        private readonly SQLiteConnection _database;

        public MapService(Microsoft.Maui.Controls.Maps.Map map, SQLiteConnection database)
        {
            _map = map;
            _database = database;
        }

        /// <summary>
        /// Updates the map with a heatmap overlay based on location data stored in the database.
        /// </summary>
        public void UpdateMapWithHeatmap()
        {
            _map.MapElements.Clear();
            var locations = _database.Table<LocationData>().ToList();
            var heatmapData = HeatmapGenerator.GenerateHeatmapData(locations);
            AddHeatmapOverlay(heatmapData);
        }

    /// function to add heatmap overalay on the saved location. 
        private void AddHeatmapOverlay(List<HeatmapDataPoint> heatmapData)
        {
            foreach (var dataPoint in heatmapData)
            {

            // Create a circle representing the heatmap data point
                var circle = new Microsoft.Maui.Controls.Maps.Circle
                {
                    Center = new Location(dataPoint.Latitude, dataPoint.Longitude),
                    Radius = new Distance(100),
                    StrokeColor = Color.FromArgb("#880000FF"),
                    StrokeWidth = 1,
                    FillColor = Color.FromArgb("#880000FF")
                };

                _map.MapElements.Add(circle);
            }
        }
    }
}