using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Report.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Services.Implamentation
{
   
        public class FileService : IFileService
        {
            private readonly string _webRootPath;
            private readonly ILogger<FileService> _logger;

            public FileService(ILogger<FileService> logger)
            {
                _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                _logger = logger;

                // اطمینان از وجود پوشه wwwroot
                if (!Directory.Exists(_webRootPath))
                {
                    Directory.CreateDirectory(_webRootPath);
                }
            }

            public async Task<string> SaveFileAsync(IFormFile file, string folder)
            {
                return await UploadFileAsync(file, folder);
            }

            public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
            {
                try
                {
                    if (file == null || file.Length == 0)
                        throw new ArgumentException("فایل معتبر نیست");

                    // اعتبارسنجی فایل
                    if (!await IsFileValidAsync(file))
                        throw new ArgumentException("نوع فایل مجاز نیست");

                    // ایجاد پوشه اگر وجود ندارد
                    var uploadsFolder = Path.Combine(_webRootPath, folderPath);
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // تولید نام فایل
                    var fileName = await GenerateFileNameAsync(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // ذخیره فایل
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var relativePath = Path.Combine(folderPath, fileName).Replace("\\", "/");
                    _logger.LogInformation("File uploaded successfully: {FilePath}", relativePath);

                    return relativePath;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file: {FileName}", file?.FileName);
                    throw new Exception("خطا در آپلود فایل", ex);
                }
            }

            public void DeleteFile(string filePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                        return;

                    var fullPath = Path.Combine(_webRootPath, filePath);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                    }
                    else
                    {
                        _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                    throw new Exception("خطا در حذف فایل", ex);
                }
            }

            public async Task<bool> DeleteFileAsync(string filePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                        return false;

                    var fullPath = Path.Combine(_webRootPath, filePath);
                    if (File.Exists(fullPath))
                    {
                        await Task.Run(() => File.Delete(fullPath));
                        _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                        return true;
                    }

                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                    return false;
                }
            }

            public async Task<byte[]> DownloadFileAsync(string filePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                        return null;

                    var fullPath = Path.Combine(_webRootPath, filePath);
                    if (File.Exists(fullPath))
                    {
                        return await File.ReadAllBytesAsync(fullPath);
                    }

                    _logger.LogWarning("File not found for download: {FilePath}", filePath);
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error downloading file: {FilePath}", filePath);
                    return null;
                }
            }

            public async Task<bool> FileExistsAsync(string filePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                        return false;

                    var fullPath = Path.Combine(_webRootPath, filePath);
                    return await Task.Run(() => File.Exists(fullPath));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking file existence: {FilePath}", filePath);
                    return false;
                }
            }

            public async Task<string> GenerateFileNameAsync(string originalFileName)
            {
                return await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(originalFileName))
                        throw new ArgumentException("نام فایل نمی‌تواند خالی باشد");

                    var extension = Path.GetExtension(originalFileName);
                    var fileName = Path.GetFileNameWithoutExtension(originalFileName);
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var random = new Random().Next(1000, 9999);

                    // پاکسازی نام فایل از کاراکترهای غیرمجاز
                    var cleanFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

                    // اگر نام فایل خالی شد، از timestamp استفاده کن
                    if (string.IsNullOrEmpty(cleanFileName))
                        cleanFileName = "file";

                    return $"{cleanFileName}_{timestamp}_{random}{extension}";
                });
            }

            private async Task<bool> IsFileValidAsync(IFormFile file)
            {
                try
                {
                    if (file == null || file.Length == 0)
                        return false;

                    // بررسی اندازه فایل (حداکثر 10 مگابایت)
                    if (file.Length > 10 * 1024 * 1024) // 10MB
                        return false;

                    // بررسی پسوند فایل
                    var allowedExtensions = new[]
                    {
                    ".pdf", ".doc", ".docx", ".xls", ".xlsx",
                    ".jpg", ".jpeg", ".png", ".gif", ".txt",
                    ".zip", ".rar", ".ppt", ".pptx"
                };

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                        return false;

                    // بررسی MIME type (اختیاری)
                    var allowedMimeTypes = new[]
                    {
                    "application/pdf",
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/vnd.ms-excel",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "image/jpeg",
                    "image/png",
                    "image/gif",
                    "text/plain",
                    "application/zip",
                    "application/x-rar-compressed",
                    "application/vnd.ms-powerpoint",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                };

                    // اگر MIME type در لیست مجاز نبود، بازهم اجازه بده (به خاطر تفاوت در مرورگرها)
                    // این بررسی اختیاری است

                    return await Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating file: {FileName}", file?.FileName);
                    return false;
                }
            }

            // متد کمکی برای دریافت اطلاعات فایل
            public async Task<long> GetFileSizeAsync(string filePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                        return 0;

                    var fullPath = Path.Combine(_webRootPath, filePath);
                    if (File.Exists(fullPath))
                    {
                        var fileInfo = new FileInfo(fullPath);
                        return await Task.FromResult(fileInfo.Length);
                    }

                    return 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting file size: {FilePath}", filePath);
                    return 0;
                }
            }

            // متد کمکی برای کپی فایل
            public async Task<string> CopyFileAsync(string sourceFilePath, string destinationFolder)
            {
                try
                {
                    if (string.IsNullOrEmpty(sourceFilePath))
                        throw new ArgumentException("مسیر فایل مبدأ نمی‌تواند خالی باشد");

                    var sourceFullPath = Path.Combine(_webRootPath, sourceFilePath);
                    if (!File.Exists(sourceFullPath))
                        throw new FileNotFoundException("فایل مبدأ یافت نشد");

                    // ایجاد پوشه مقصد اگر وجود ندارد
                    var destFolderPath = Path.Combine(_webRootPath, destinationFolder);
                    if (!Directory.Exists(destFolderPath))
                        Directory.CreateDirectory(destFolderPath);

                    // تولید نام جدید برای فایل کپی
                    var originalFileName = Path.GetFileName(sourceFilePath);
                    var newFileName = await GenerateFileNameAsync(originalFileName);
                    var destFilePath = Path.Combine(destFolderPath, newFileName);

                    // کپی فایل
                    await Task.Run(() => File.Copy(sourceFullPath, destFilePath, true));

                    var relativePath = Path.Combine(destinationFolder, newFileName).Replace("\\", "/");
                    _logger.LogInformation("File copied successfully from {Source} to {Destination}",
                        sourceFilePath, relativePath);

                    return relativePath;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error copying file from {Source} to {Destination}",
                        sourceFilePath, destinationFolder);
                    throw new Exception("خطا در کپی فایل", ex);
                }
            }
        }
    }
