using System.Collections.Generic;

namespace MovieReview.Model
{
    public class ContentResponse
    {
        public List<Content> Search { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
    }
}
