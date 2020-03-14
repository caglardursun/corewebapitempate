using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Helpers
{
    public static class FileHelper
    {

        public static string ToFileExtension(this string fileName)
        {
            return fileName.Split(".")[fileName.Split(".").Length - 1];
        }

        /// <summary>
        /// Accepted File format
        /// </summary>
        public enum SupportedFileFormat
        {
            Bmp,
            Jpeg,            
            Tiff,
            Png,
            Pdf, //
            Doc, // 
            Docx, //
            Xlsx,
            Xls,
            Zip,
            Rar,
            Unknown
        }
        public static SupportedFileFormat GetSupportedFormat(this string fileName)
        {
            switch (fileName.Split(".")[fileName.Split(".").Length - 1].ToLower())
            {
                case "bmp":
                    return SupportedFileFormat.Bmp;
                case "jpg":
                    return SupportedFileFormat.Jpeg;
                case "jpeg":
                    return SupportedFileFormat.Jpeg;
                case "tiff":
                    return SupportedFileFormat.Tiff;
                case "tif":
                    return SupportedFileFormat.Tiff;                
                case "png":
                    return SupportedFileFormat.Png;
                case "pdf":
                    return SupportedFileFormat.Pdf;
                case "doc":
                    return SupportedFileFormat.Doc;
                case "docx":
                    return SupportedFileFormat.Docx;
                case "zip":
                    return SupportedFileFormat.Zip;
                case "rar":
                    return SupportedFileFormat.Rar;
                case "xls":
                    return SupportedFileFormat.Xls;
                case "xlsx":
                    return SupportedFileFormat.Xlsx;
                default:
                    return SupportedFileFormat.Unknown;                    
            }            
        }

        public static string ToMimeTypes(this string fileExtension)
        {            
            return GetMimeTypes()[fileExtension.ToFileExtension()];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {"txt", "text/plain"},
                {"zip", "application/zip"},
                {"pdf", "application/pdf"},                
                {"doc", "application/vnd.ms-word"},
                {"docx", "application/vnd.ms-word"},
                {"xls", "application/vnd.ms-excel"},
                {"xlsx", "application/vnd.openxmlformats"},                
                {"png", "image/png"},
                {"jpg", "image/jpeg"},
                {"tiff", "image/tiff"},
                {"jpeg", "image/jpeg"},
                {"gif", "image/gif"},
                {"csv", "text/csv"}
            };
        }

    }
}
