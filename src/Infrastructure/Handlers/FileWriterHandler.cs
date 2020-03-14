using API.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using API.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Spire.Xls;
using API.Data.Entity;
using System.Linq;
using System.Globalization;
using API.DTO.Request;
using FluentValidation.Results;
using AutoMapper;

namespace API.Infrastructure.Handlers
{
    public class FileWriterHandler : IFileWriterHandler
    {

        private string UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        private ILogger<FileWriterHandler> _logger;
        private IRehberManager _rehberManager;
        private IMapper _mapper;

        public FileWriterHandler(
            IRehberManager rehberManager,
            IMapper mapper,
            ILogger<FileWriterHandler> logger
            )
        {
            _logger = logger;
            _rehberManager = rehberManager;
            _mapper = mapper;
        }
        private string GetFullPath(string file)
        {
            return Path.Combine(UploadPath, file);
        }


        public Task<bool> DeleteFileAsync(IFormFile file)
        {
            return DeleteFileAsync(file.FileName);
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {

            try
            {
                if (File.Exists(GetFullPath(filePath)))
                {
                    File.Delete(GetFullPath(filePath));
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An Error Occured When Deleting The File {0}", filePath);
                throw exc;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {

            try
            {
                if (file == null)
                    throw new ArgumentNullException("No file found");

                //time span + bla bla
                DateTime now = DateTime.Now;

                //check extension is valid or not !!!
                if (file.FileName.GetSupportedFormat() == FileHelper.SupportedFileFormat.Unknown)
                    throw new FileLoadException("Unsported file format");


                string fileName = string.Format("{0}_{1}_{2}", Guid.NewGuid().ToString("N"), now.ToString("dd-MM-yyyy_hh-mm-ss"), file.FileName);


                if (!Directory.Exists(UploadPath))
                    Directory.CreateDirectory(UploadPath);


                string filePath = Path.Combine(UploadPath, fileName);

                if (File.Exists(filePath))
                    throw new Exception("File already exists");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An Error Occured When Uploading The File {0}", file.FileName);
                throw exc;
            }
        }

        public async Task<FileStream> GetFileAsync(string file)
        {
            if (!File.Exists(GetFullPath(file)))
                throw new FileNotFoundException(string.Format("The file {0} does not exists", file));


            return File.OpenRead(GetFullPath(file));
        }

        public async Task<string> GetBase64(IFormFile file)
        {

            string fileName = await UploadFileAsync(file);
            string path = GetFullPath(fileName);
            byte[] bytes = File.ReadAllBytes(path);

            string base64 = Convert.ToBase64String(bytes);
            //Ulan az önce oluşturdun dosyayı
            //Now delete the file 
            if (File.Exists(path))
                File.Delete(path);

            return base64;

        }

        public async Task<Dictionary<string, IEnumerable<string>>> GetExcelData(string fileName)
        {

            string path = GetFullPath(fileName);
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(path);

            var content = new Dictionary<string, IEnumerable<string>>();

            List<string> sheets = new List<string>();

            foreach (var item in workbook.Worksheets)
            {
                sheets.Add(item.CodeName);
            }
            content["Worksheets"] = sheets;
            var worksheet = workbook.Worksheets[0];

            List<string> headers = new List<string>();



            foreach (var item in worksheet.Rows[0].Cells)
            {
                headers.Add(item.DisplayedText);
            }

            content["Headers"] = headers;

            for (int i = 1; i < 4; i++)
            {
                var list = new List<string>();
                foreach (var item in worksheet.Rows[i].Cells)
                {
                    list.Add(item.DisplayedText);
                }
                content[$"Row-{i}"] = list;
            }

            return content;

        }

        public async Task<bool> ImportExcel(string FileName, long CustomerID, long PersonID, IEnumerable<string> fields)
        {
            try
            {
                string filePath = GetFullPath(FileName);


                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"{filePath} does not exists");

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(filePath);
                var validator = new CreateRehberRequestValidator();
                //Assume first row is header
                for (int sayfa = 0; sayfa < workbook.Worksheets.Count; sayfa++)
                {
                    var worksheet = workbook.Worksheets[sayfa];

                    for (int satir = 1; satir < worksheet.Rows.Length; satir++)
                    {
                        //Assume you may face a problem with this record
                        //then you have to skip this row
                        try
                        {
                            var columns = worksheet.Rows[satir].Cells;
                            CreateRehberRequest r = new CreateRehberRequest();
                            r.company_id = CustomerID;
                            r.person_id = PersonID;

                            for (int cell = 0; cell < fields.Count(); cell++)
                            {
                                //Email Validation ??? 
                                //the field is datetime or not !
                                if (fields.ElementAt(cell).EndsWith("_day"))
                                {
                                    DateTime dt = DateTime.Parse(columns.ElementAt(cell).DisplayedText);
                                    typeof(CreateRehberRequest).GetProperty(fields.ElementAt(cell)).SetValue(r, dt);
                                }
                                else
                                {
                                    typeof(CreateRehberRequest).GetProperty(fields.ElementAt(cell)).SetValue(r, columns.ElementAt(cell).DisplayedText);
                                }
                            }

                            //Validate the model (required field etc)
                            if ((await validator.ValidateAsync(r)).IsValid)
                            {
                                //Prevend inserting existing email record
                                if (!await _rehberManager.ExistAsync(r.email))
                                {
                                    var rehberEntity = _mapper.Map<Rehber>(r);
                                    await _rehberManager.CreateAsync(rehberEntity);
                                }
                            }
                        }
                        catch
                        {
                            //Do not break the loop keep going
                            continue;
                        }
                    }

                }

                return true;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
