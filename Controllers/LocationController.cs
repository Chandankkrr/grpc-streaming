using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GPRCStreaming
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        public readonly FileReader _fileReader;
        public readonly ILogger<LocationService> _logger;

        public LocationController(FileReader fileReader, ILogger<LocationService> logger)
        {
            _fileReader = fileReader;
            _logger = logger;
        }

        [HttpGet("getLocationData")]
        public async Task<IEnumerable<Location>> GetLocationData([FromQuery] GetLocationsRequest request)
        {
            var locationData = await ReadLocationData();

            return locationData.Locations.Take(request.DataLimit);
        }

        [HttpGet("getAllLocationData")]
        public async Task<IEnumerable<Location>> GetAllLocationData(GetAllLocationsRequest request)
        {
            var locationData = await ReadLocationData();

            return locationData.Locations;
        }

        private async Task<RootLocation> ReadLocationData()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = $"{currentDirectory}/Data/Location_History.json";

            var locationData = await _fileReader.ReadAllLinesAsync(filePath);

            return locationData;
        }
    }
}
