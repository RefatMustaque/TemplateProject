using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TemplateProject.Helpers
{
    public class FileHelper
    {
        private IHostingEnvironment _hostingEnvironment;

        private DateTimeHelper _dateTimeHelper { get; set; }

        public FileHelper(IHostingEnvironment environment, DateTimeHelper dateTimeHelper)
        {
            _hostingEnvironment = environment;

            _dateTimeHelper = dateTimeHelper;
        }


        public async Task<byte[]> FormFileToByteArray(IFormFile file)
        {
            byte[] byteConvertedFile = new byte[0];

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                byteConvertedFile = memoryStream.ToArray();
            }

            return byteConvertedFile;
        }

        public async Task<string> SaveToFolderAndReturnPath(IFormFile file)
        {


            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            var filePath = "";

            if (file.Length > 0)
            {
                string dateTimeUtcNow = _dateTimeHelper.BDDateTime().ToString("MM-dd-yyyy HH-mm-ss-fff");

                filePath = Path.Combine(uploadPath, dateTimeUtcNow + file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            var relativePath = GetAsApplicationWebrootPath(filePath);

            return relativePath;
        }

        public void RemoveFileOfPath(string path)
        {
            if (path == null)
            {
                path = "";
            }

            var uploadedPath = _hostingEnvironment.ContentRootPath + path;

            if (File.Exists(uploadedPath))
            {
                // If file found, delete it    
                File.Delete(uploadedPath);
            }
        }

        private string GetAsApplicationWebrootPath(string path)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;

            var relativePath = path.Replace(contentRootPath, "");

            return relativePath;
        }

        internal string GetFilePathAsSourceUrl(string directoryPath)
        {
            var sourceUrl = directoryPath.Replace(@"\wwwroot", "");

            sourceUrl = sourceUrl.Replace(@"\", "/");

            return sourceUrl;
        }

        internal string GetFileExtensionOfPath(string fileDirectoryLink)
        {
            return Path.GetExtension(fileDirectoryLink);
        }
    }
}
