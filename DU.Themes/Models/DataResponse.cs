using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Models
{
    public class DataResponse<T> : ISortable
        where T : class, new()
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public int Total { get; set; }

        public IEnumerable<T> Data { get; set; }

        public DataResponse(IEnumerable<T> source, int take, int skip, int total)
        {
            this.Data = source;
            this.Take = take;
            this.Skip = skip;
            this.Total = total;
        }
    }
}