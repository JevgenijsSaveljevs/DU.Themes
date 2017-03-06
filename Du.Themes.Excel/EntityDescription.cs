using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Du.Themes.Excel
{
    public class EntityDescription
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EntityMapping this[string index]
        {
            get
            {
                return this.Fields.FirstOrDefault(x => x.ColumnName == index);
            }
        }

        ICollection<EntityMapping> Fields { get; set; }

        public EntityDescription(params EntityMapping[] mappings)
        {
            this.Fields = new List<EntityMapping>();

            if (mappings != null)
            {
                this.Fields = mappings
                    .Select(x => new EntityMapping
                        {
                            ColumnName = x.ColumnName.TrimStart().TrimEnd(),
                            EntityName = x.EntityName.TrimStart().TrimEnd()
                        })
                    .ToList();
            }
        }
    }

    public class EntityMapping
    {
        public string EntityName { get; set; }
        public string ColumnName { get; set; }
    }
}