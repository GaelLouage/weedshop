using Infrastructuur.Database.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Database
{
    public class FileService : IFileService
    {
        public async Task<string> UploadImage(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(@"wwwroot\WeedImg\", fileName).Replace("\\", "/");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filePath.Replace("wwwroot", "");
        }
    }
}
