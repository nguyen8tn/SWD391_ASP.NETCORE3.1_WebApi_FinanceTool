using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class Filter
    {
        public String Term { get; set; }
        public DateTime MinDate { get; set; }
        public Boolean IncludeInactive { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
    public class PagedCollectionResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}
