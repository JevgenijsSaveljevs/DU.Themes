using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Infrastructure.Comparers
{
    public class ModelIdComparer<T> : IEqualityComparer<T>
        where T : Models.ModelBase
    {
        public bool Equals(T x, T y)
        {
            if(x.Id == y.Id)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(T obj)
        {
            return Convert.ToInt32(obj.Id);
        }
    }
}