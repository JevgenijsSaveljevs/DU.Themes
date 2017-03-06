using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DU.Themes
{
    [DataContract]
    public class DataTablesRequest
    {
        public DataTablesRequest()
        {
            this.Columns = new List<DataTablesColumnDefinition>();
        }

        [DataMember(Name = "draw")]
        public string Draw { get; set; }

        [DataMember(Name = "length")]
        public int Length { get; set; }

        [DataMember(Name = "start")]
        public int Start { get; set; }

        [DataMember(Name = "search")]
        public DataTablesSearch Search { get; set; }

        [DataMember(Name = "columns")]
        public IEnumerable<DataTablesColumnDefinition> Columns { get; set; }

        [DataMember(Name = "order")]
        public IEnumerable<DataTablesOrder> Order { get; set; }

        public static DataTablesRequest TryParse(string queryString)
        {
            NameValueCollection qscoll = HttpUtility.ParseQueryString(queryString);

            return null;
        }

        public string OrderBy
        {
            get
            {
                return this.Columns.ElementAt(this.Order.First().Column).Data;
            }
        }

        public bool OrderAscending
        {
            get
            {
                return this.Order.First().Dir == "asc";
            }
        }
    }

    [DataContract]
    public class DataTablesSearch
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "regex")]
        public string Regex { get; set; }
    }

    [DataContract]
    public class DataTablesOrder
    {
        [DataMember(Name = "column")]
        public int Column { get; set; }

        [DataMember(Name = "dir")]
        public string Dir { get; set; }
    }

    [DataContract]
    public class DataTablesColumnDefinition
    {
        [DataMember(Name = "data")]
        public string Data { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "searchable")]
        public bool Searchable { get; set; }

        [DataMember(Name = "orderable")]
        public string Orderable { get; set; }

        [DataMember(Name = "search")]
        public string Search { get; set; }
    }
}