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
        const string BASE_FOLDER = "Uploads";

        private (string fullPath, string fullPathWithoutCurrentDirectory) GetDirectoryUpload(string? path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("Caminho não especificado.");

            var pathCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var pathUpload = @$"{BASE_FOLDER}/{path}";

#if DEBUG
            pathUpload = @$"{BASE_FOLDER}\{path.Replace("/", @"\")}";
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
                            Path = uploadFile.Path,
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

        public async Task<BusinessResult<FileStream>> GetAsync(string path)
        {
            try
            {
                var response = BusinessResult<FileStream>.CreateValidResult();

                if (string.IsNullOrWhiteSpace(path))
                    return await Task.FromResult(BusinessResult<FileStream>.CreateInvalidResult("O caminho não pode ser vazio."));

                var filePath = GetDirectoryUpload(path).fullPath.Replace("'", "");
                response.Data = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(BusinessResult<FileStream>.CreateInvalidResult(ex));
            }
        }

        public async Task<BusinessResult<List<UploadFileResponse>>> GetFilesAsync(string path)
        {
            return await GetFiles(path);
        }

        public async Task<BusinessResult<List<UploadFileResponse>>> GetFilesWithBaseUrlAsync(string baseUrl, string path)
        {
            return await GetFiles(path, baseUrl);
        }

        private async Task<BusinessResult<List<UploadFileResponse>>> GetFiles(string path, string? baseUrl = null)
        {
            try
            {
                var response = BusinessResult<List<UploadFileResponse>>.CreateValidResult(new List<UploadFileResponse>());

                string fullPath = GetDirectoryUpload(path).fullPath.Replace("'", "");
                if (!Directory.Exists(fullPath))
                {
                    return await Task.FromResult(BusinessResult<List<UploadFileResponse>>.CreateInvalidResult("O caminho especificado não existe."));
                }

                foreach (var filePath in Directory.EnumerateFiles(fullPath, "*.jpeg"))
                {
                    response.Data.Add(new UploadFileResponse()
                    {
                        Path = $"{baseUrl}{(filePath.Split($"{BASE_FOLDER}")[1]).Replace("\\", "/").Replace(@"\", "/")}",
                        NameFile = Path.GetFileName(filePath),
                    });
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
