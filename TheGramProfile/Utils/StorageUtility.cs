using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NLog;

namespace TheGramProfile
{
    public class StorageUtility: IStorageUtility
    {
        private readonly IWebHostEnvironment _env;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public StorageUtility(IWebHostEnvironment env)
        {
            this._env = env;
        }
        
        public async Task<FileStream> CreateFile(IFormFile file)
        {
            FileStream fs = null;
            // upload file
            string folderName = "temporary_media_for_upload";
            string path = Path.Combine(_env.ContentRootPath, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
            await file.CopyToAsync(fs);
            try
            {
                return new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteFile(FileStream path)
        {
            try
            {
                File.Delete(path.Name);
                await path.DisposeAsync();
            }
            catch (ArgumentNullException e)
            {
                Logger.Error(e,"Filestream supplied is null");
            }
        }
    }
}