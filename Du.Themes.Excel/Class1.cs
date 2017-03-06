using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace Du.Themes.Excel
{
    public class ExcelSomething<T>
        where T : class, new()
    {
        /// <summary>
        /// Column index, Column Name 
        /// </summary>
        Dictionary<int, string> nameMap = new Dictionary<int, string>();
        IEnumerable<PropertyInfo> Properties;

        public Stream Stream { get; private set; }
        public EntityDescription Description { get; private set; }

        public string this[int index]
        {
            get
            {
                if (!nameMap.ContainsKey(index))
                {
                    return string.Empty;
                }

                return nameMap[index];
            }
        }


        public ExcelSomething(Stream inputStream, EntityDescription description)
        {
            this.Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            this.Stream = inputStream;
            this.Description = description;
        }

        public List<T> ReadFromExcel()
        {
            var result = new List<T>();
            using (var package = new ExcelPackage(this.Stream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();

                var colFirst = workSheet.Dimension.Start.Column;
                var colLast = workSheet.Dimension.End.Column;
                var rowFirst = workSheet.Dimension.Start.Row;
                var rowLast = workSheet.Dimension.End.Row;

                // Header                
                for (int i = colFirst; i <= colLast; i++)
                {
                    var cell = workSheet.Cells[rowFirst, i];
                    nameMap.Add(i, cell.Text.TrimStart().TrimEnd());
                }

                for (int row = rowFirst + 1; row <= rowLast; row++)
                {
                    var entity = new T();
                    for (int column = colFirst; column <= colLast; column++)
                    {
                        this.SafeSet(entity, workSheet, this.Description, row, column);
                    }

                    result.Add(entity);
                }
            }

            return result;
        }


        private void SafeSet(T entity, ExcelWorksheet workSheet, EntityDescription description, int row, int column)
        {
            var excelName = this[column];
            var descr = description[excelName];

            if (descr == null)
            {
                return;
            }

            var prop = this.Properties.First(x => x.Name == descr.EntityName);

            var setter = prop?.SetMethod;

            var typeCode = Type.GetTypeCode(prop.PropertyType);

            if (setter != null)
            {
                setter.Invoke(entity, new[] { Convert.ChangeType(workSheet.Cells[row, column].Value, typeCode) });
            }
        }
    }
}

