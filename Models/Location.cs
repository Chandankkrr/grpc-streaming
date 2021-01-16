using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapboxStreaming
{
    public record Location
    {
        [JsonProperty("latitudeE7")]
        public int LatitudeE7 { get; set; }

        [JsonProperty("longitudeE7")]
        public int LongitudeE7 { get; set; }
    }

    public class RootLocation
    {
        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }
    }
}