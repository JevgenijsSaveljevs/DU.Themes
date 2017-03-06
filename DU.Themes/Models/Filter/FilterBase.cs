using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Models.Filter
{
    public class FilterBase
    {
        public bool SortByAscending
        {
            get
            {
                if(this.SortOrder == -1)
                {
                    return true;
                }

                return false;
            }
        }

        public string Search { get; set; }



        public int SortOrder { get; set; }

        private string sortKey;
        public string SortKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.sortKey))
                {
                    return nameof(ModelBase.Id);
                }
                else
                {
                    return this.sortKey.TrimEnd().TrimStart();
                }
            }
            set
            {
                sortKey = value;
            }
        }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}