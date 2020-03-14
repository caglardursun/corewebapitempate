using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Contracts
{
       
    public interface IFileWriterHandler
    {
        Task<FileStream> GetFileAsync(string file);
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> GetBase64(IFormFile file);

        Task<bool> DeleteFileAsync(string filePath);        

    }
}
