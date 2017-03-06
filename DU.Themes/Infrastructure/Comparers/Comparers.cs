using DU.Themes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Infrastructure.Comparers
{
    public static class Comparers<T>
        where T : ModelBase
    {
        public static ModelIdComparer<T> ModelIdComparer = new ModelIdComparer<T>();
    }
}