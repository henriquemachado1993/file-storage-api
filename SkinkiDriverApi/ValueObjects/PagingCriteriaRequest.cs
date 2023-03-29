using System.Runtime.Serialization;

namespace SkinkiDriverApi.ValueObjects
{
    [DataContract]
    public class PagingCriteriaRequest
    {
        public PagingCriteriaRequest() {
            OrderBy = new List<string>();
            Navigation = new List<string>();
        }

        [DataMember]
        public int Limit { get; set; }
        [DataMember]
        public int Offset { get; set; }
        [DataMember]
        public List<string> OrderBy { get; set; }
        [DataMember]
        public List<string> Navigation { get; set; }
    }
}
