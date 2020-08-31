using System.Collections.Generic;

namespace NewsSource.Shared.Models
{
    public class NewsResponse
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
