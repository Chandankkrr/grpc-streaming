using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrpcStreaming.Models;
using GrpcStreaming.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrpcStreaming.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly FileReader _fileReader;
        private readonly ILogger<LocationController> _logger;

        public LocationController(FileReader fileReader, ILogger<LocationController> logger)
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
