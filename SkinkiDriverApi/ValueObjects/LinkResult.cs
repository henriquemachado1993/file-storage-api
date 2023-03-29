using System;
using System.Collections.Generic;
using System.Text;

namespace SkinkiDriverApi.ValueObjects
{
    public class LinkResult
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
        public bool? IsTemplate { get; set; }
    }
}
