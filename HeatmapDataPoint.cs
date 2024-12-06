using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationTracker
{
    public class HeatmapDataPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Intensity { get; set; }
    }

    public static class HeatmapGenerator
    {
        public static List<HeatmapDataPoint> GenerateHeatmapData(List<LocationData> locations)
        {
            var heatmapData = new List<HeatmapDataPoint>();

            // Aggregate locations and assign intensity values
            foreach (var loc in locations)
            {
                heatmapData.Add(new HeatmapDataPoint
                {
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    Intensity = 1.0 
                });
            }

            return heatmapData;
        }
    }
}
