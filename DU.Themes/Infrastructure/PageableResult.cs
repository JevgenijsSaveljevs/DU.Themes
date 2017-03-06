using DU.Themes.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DU.Themes.Infrastructure
{
    [DataContract]
    public class PageableResult<T>
        where T : ModelBase
    {
        public PageableResult(int count, IEnumerable<T> data, int page)
        {
            this.Count = count;
            this.Items = data;
            this.Page = page;
        }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "items")]
        public IEnumerable<T> Items { get; set; }

        [DataMember(Name = "page")]
        public int Page { get; private set; }
    }
}