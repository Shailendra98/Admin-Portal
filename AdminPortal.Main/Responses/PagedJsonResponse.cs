using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Responses
{
    public class PagedJsonResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
