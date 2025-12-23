using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        void DeleteFile(string filePath);
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> DownloadFileAsync(string filePath);
        Task<bool> FileExistsAsync(string filePath);
        Task<string> GenerateFileNameAsync(string originalFileName);
    }
}
