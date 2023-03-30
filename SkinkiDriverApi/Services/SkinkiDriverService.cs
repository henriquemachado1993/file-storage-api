using Newtonsoft.Json;
using SkinkiDriverApi.Interfaces;
using SkinkiDriverApi.Models;
using SkinkiDriverApi.ValueObjects;
using System.Reflection;
using System.Text;

namespace SkinkiDriverApi.Services
{
    public class SkinkiDriverService : ISkinkiDriverService
    {
        private (string fullPath, string fullPathWithoutCurrentDirectory) GetDirectoryUpload(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("Caminho não especificado.");

            var pathCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var pathUpload = @$"Uploads/{path}";

#if DEBUG
            pathUpload = @$"Uploads\{path.Replace("/", @"\")}";
            pathCurrentDirectory = Directory.GetCurrentDirectory();
#endif


            return (Path.Combine(pathCurrentDirectory, pathUpload), pathUpload);
        }

        public async Task<BusinessResult<List<UploadFileResponse>>> UploadAsync(UploadFileRequest uploadFile)
        {
            try
            {
                var response = BusinessResult<List<UploadFileResponse>>.CreateValidResult(new List<UploadFileResponse>());

                if (string.IsNullOrWhiteSpace(uploadFile.Path))
                    return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult("O caminho do arquivo não pode ser vazio."));

                string fullPath = GetDirectoryUpload(uploadFile.Path).fullPath.Replace("'", "");
                string fullPathWithoutCurrentDirectory = GetDirectoryUpload(uploadFile.Path).fullPathWithoutCurrentDirectory;

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                if (uploadFile.BytesFiles != null && uploadFile.BytesFiles.Any())
                {
                    var formFiles = new List<IFormFile>();
                    var nameFile = string.Empty;
                    foreach (var bytesFile in uploadFile.BytesFiles)
                    {
                        nameFile = Guid.NewGuid().ToString();
                        string fileExtension = Path.GetExtension($"{nameFile}." + Encoding.Default.GetString(bytesFile));

                        // TODO: ver uma forma de pegar a extensão do arquivo através dos bytes 
                        formFiles.Add(ConvertByteArrayToIFormFile(bytesFile, $"{nameFile}.jpeg"));
                    }

                    foreach (var formFile in formFiles)
                    {
                        fullPath = Path.Combine(fullPath, formFile.FileName);
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);
                            fileStream.Close();
                        }

                        response.Data.Add(new UploadFileResponse()
                        {
                            Path = fullPathWithoutCurrentDirectory,
                            NameFile = formFile.FileName
                        });
                    }
                }
                else
                {
                    return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult("Nenhum arquivo encontrado para realizar o upload."));
                }

                return await Task.FromResult(response);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult(ex));
            }
        }

        private IFormFile ConvertByteArrayToIFormFile(byte[] bytes, string fileName)
        {
            var stream = new MemoryStream(bytes);
            var formFile = new FormFile(stream, 0, bytes.Length, null, fileName);
            return formFile;
        }

        public async Task<BusinessResult<UploadFileResponse>> GetAsync(string path)
        {
            try
            {
                var response = BusinessResult<UploadFileResponse>.CreateValidResult();
                var currentDirectory = Directory.GetCurrentDirectory();
                var imagePath = Path.Combine("Uploads", path);
                var files = Directory.GetFiles(imagePath);

                var fileObjects = new List<object>();

                foreach (var filePath in files)
                {
                    var fileInfo = new FileInfo(filePath);
                    var fileObject = new
                    {
                        Name = fileInfo.Name,
                        Size = fileInfo.Length,
                        Created = fileInfo.CreationTimeUtc,
                        Modified = fileInfo.LastWriteTimeUtc
                    };
                    fileObjects.Add(fileObject);
                }

                var json = JsonConvert.SerializeObject(fileObjects);
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BusinessResult<UploadFileResponse>.CreateInvalidResult(ex));
            }
        }

        public async Task<BusinessResult<List<UploadFileResponse>>> GetFilesAsync(string path)
        {
            try
            {
                var response = BusinessResult<List<UploadFileResponse>>.CreateValidResult(new List<UploadFileResponse>());

                string fullPath = GetDirectoryUpload(path).fullPath;
                if (!Directory.Exists(fullPath))
                {
                    return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult("O caminho especificado não existe."));
                }



                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult(ex));
            }
        }
    }
}
