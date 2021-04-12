using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStorage
{
    public class FileDataStorage<TObject> where TObject : class, IStorable
    {
        private static readonly string BaseFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BudgetsStorage",
                typeof(TObject).Name);

        public FileDataStorage()
        {
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
        }

        public async Task AddOrUpdateAsync(TObject obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            await using var sw = new StreamWriter(Path.Combine(BaseFolder, obj.Guid.ToString()), false);
            await sw.WriteAsync(stringObj);
        }

        public async Task<TObject> GetAsync(Guid guid)
        {
            string stringObj = null;
            string filePath = Path.Combine(BaseFolder, guid.ToString());

            if (!File.Exists(filePath))
                return null;

            using (StreamReader sw = new StreamReader(filePath))
            {
                stringObj = await sw.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<TObject>(stringObj);
        }

        public async Task<List<TObject>> GetAllAsync()
        {
            var res = new List<TObject>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;

                using (var sw = new StreamReader(file))
                {
                    stringObj = await sw.ReadToEndAsync();
                }

                var item = JsonSerializer.Deserialize<TObject>(stringObj);
                string guid = item.Guid.ToString();
                res.Add(item);
            }

            return res;
        }
    }
}