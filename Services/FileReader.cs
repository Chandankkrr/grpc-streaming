using System.IO;
using System.Threading.Tasks;
using GrpcStreaming.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrpcStreaming.Services
{
    public class FileReader
    {
        private readonly ILogger<FileReader> _logger;

        private RootLocation _locationData;

        public FileReader(ILogger<FileReader> logger)
        {
            _logger = logger;
        }

        public async Task<RootLocation> ReadAllLinesAsync(string filePath)
        {
            if (_locationData != null) return _locationData;
            
            var serializer = new JsonSerializer();

            await using (var fileStream = File.Open(filePath, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                while (await reader.ReadAsync())
                {
                    _logger.LogInformation("Reading contents of {FilePath} file", filePath);
                    
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        _locationData = serializer.Deserialize<RootLocation>(reader);
                    }
                }
                
                _logger.LogInformation("{Count} records found", _locationData!.Locations.Count.ToString());
            }
            
            return _locationData;
        }
    }
}