using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaTracker.API.Utils
{
    public class JsonFileManager<T> where T : class
    {
        private readonly string _filePath;

        public JsonFileManager(string filePath)
        {
            _filePath = filePath;

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<List<T>> ReadAllAsync()
        {
            using var fileStream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<List<T>>(fileStream) ?? new List<T>();
        }

        public async Task WriteAllAsync(List<T> items)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using var fileStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fileStream, items, options);
        }
    }
}