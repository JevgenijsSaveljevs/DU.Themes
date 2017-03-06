using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSortingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var persons = CreatePersonList().AsQueryable<Person>();

            //            persons.Sort()

            var sorted = GenericSortingTest.Extensions.Sort<Person>(persons, "Work.Id");

            foreach (var person in sorted) 
            {
                Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, Work.Id {person.Work.Id}, Work.JobName: {person.Work.JobName}");
            }

            Console.ReadKey();
        }

        private static List<Person> CreatePersonList()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "Jevgenijs",
                    Work = new Work
                    {
                        JobName = "Programmer",
                        Id = 1,
                        Sallary = 700
                    }
                },
                new Person
                {
                    Id = 2,
                    Name = "Edgars",
                    Work = new Work
                    {
                        JobName = "Lumberjack",
                        Id = 2,
                        Sallary = 1700
                    }
                },
                new Person
                {
                    Id = 3,
                    Name = "Ryan",
                    Work = new Work
                    {
                        JobName = "Singer",
                        Id = 3,
                        Sallary = 3700
                    }
                },
                new Person
                {
                    Id = 4,
                    Name = "Bruce",
                    Work = new Work
                    {
                        JobName = "Programmer",
                        Id = 1,
                        Sallary = 1700
                    }
                },
            };
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Work Work { get; set; }
    }

    public class Work
    {
        public int Id { get; set; }
        public int Sallary { get; set; }
        public string JobName { get; set; }
    }
}
