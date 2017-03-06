using Du.Themes.Excel;
using DU.Themes.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DU.Themes.Infrastructure.Excel
{
    public static class ExcelHelper
    {
        public static EntityMapping[] DefinitionFromConfig()
        {
            var section = (PropertyMappings)System.Configuration.ConfigurationManager.GetSection("ExcelImport");
            var result = new List<EntityMapping>();

            foreach (PropertyElement prop in section.DefinedProperties)
            {
                result.Add(new EntityMapping
                {
                    ColumnName = prop.ExcelName,
                    EntityName = prop.PocoName
                });
            }

            return result.ToArray();
        }
    }
}