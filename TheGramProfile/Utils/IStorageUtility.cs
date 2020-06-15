using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TheGramProfile
{
    public interface IStorageUtility
    {
        public Task<FileStream> CreateFile(IFormFile file);
        public Task DeleteFile(FileStream path);
    }
}