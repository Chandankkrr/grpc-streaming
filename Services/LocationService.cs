using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.IO;

namespace GPRCStreaming
{
    public class LocationService : LocationData.LocationDataBase
    {
        public readonly FileReader _fileReader;
        public readonly ILogger<LocationService> _logger;

        public LocationService(FileReader fileReader, ILogger<LocationService> logger)
        {
            _fileReader = fileReader;
            _logger = logger;
        }

        public override async Task GetLocationData(GetLocationRequest request, IServerStreamWriter<GetLocationResponse> responseStream, ServerCallContext context)
        {
            var locationData = await ReadLocationData();

            for (var i = 0; i < request.DataLimit - 1; i++)
            {
                var item = locationData.Locations[i];

                await responseStream.WriteAsync(new GetLocationResponse
                {
                    LatitudeE7 = item.LatitudeE7,
                    LongitudeE7 = item.LongitudeE7
                });
            }
        }

        public override async Task GetAllLocationData(GetAllLocationsRequest request, IServerStreamWriter<GetAllLocationsResponse> responseStream, ServerCallContext context)
        {
            var locationData = await ReadLocationData();
            var locations = locationData.Locations;

            foreach (var item in locations)
            {
                await responseStream.WriteAsync(new GetAllLocationsResponse
                {
                    LatitudeE7 = item.LatitudeE7,
                    LongitudeE7 = item.LongitudeE7
                });
            }
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