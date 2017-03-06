using Du.Themes.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "jĒvģĒņīš";
            var name1 = name.Normalize();
            var name2 = name.Normalize(NormalizationForm.FormC);
            var name3 = name.Normalize(NormalizationForm.FormD);
            var name4 = name.Normalize(NormalizationForm.FormKC);
            var name5 = name.Normalize(NormalizationForm.FormKD);
            var english = Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(name));

            Console.WriteLine(name);
            Console.WriteLine(english);
            Console.WriteLine(name.Normalize());
            // name.Normalize(NormalizationForm.)
            Console.WriteLine(name.Normalize(NormalizationForm.FormC));
            Console.WriteLine(name.Normalize(NormalizationForm.FormD));

            Console.WriteLine(name.Normalize(NormalizationForm.FormKC));
         
            Console.WriteLine(name.Normalize(NormalizationForm.FormKD));   //var path = @"F:\Soft\DU.Themes\ExcelTest\data - Copy.xlsx";

            //// Open the stream and read it back.
            //using (FileStream fs = File.Open(path, FileMode.Open))
            //{
            //    byte[] b = new byte[1024];
            //    UTF8Encoding temp = new UTF8Encoding(true);

            //    while (fs.Read(b, 0, b.Length) > 0)
            //    {
            //        Console.WriteLine(temp.GetString(b));
            //    }

            //    var mappings = new List<EntityMapping>();
            //    var section = (DU.Themes.Configuration.PropertyMappings)System.Configuration.ConfigurationManager.GetSection("ExcelImport");
            //    foreach (DU.Themes.Configuration.PropertyElement prop in section.DefinedProperties)
            //    {
            //        mappings.Add(new EntityMapping
            //        {
            //            ColumnName = prop.ExcelName,
            //            EntityName = prop.PocoName
            //        });
            //    }

            //    var smth = new ExcelSomething<Person>(fs, new EntityDescription(
            //        new EntityMapping
            //        {
            //            ColumnName = "Vārds",
            //            EntityName = nameof(Person.FirstName)
            //        },
            //        new EntityMapping
            //        {
            //            ColumnName = "Programmas nos. ",
            //            EntityName = nameof(Person.ProgramName)
            //        }));

            //    smth.ReadFromExcel();

            //}


            Console.ReadKey();

        }

        public class Person
        {
            public int Age { get; set; }
            public string FirstName { get; set; }
            public string ProgramName { get; set; }
        }
    }
}
