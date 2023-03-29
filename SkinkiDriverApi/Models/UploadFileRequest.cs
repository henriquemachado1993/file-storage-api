using System.Text.Json.Serialization;

namespace SkinkiDriverApi.Models
{
    public class UploadFileRequest
    {
        public string Path { get; set; }
        public List<byte[]> BytesFiles { get; set; }
    }
}
