using System.Data.Entity;

namespace DU.Themes
{
    public partial class DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Customer>()
            //    .HasIndex("IX_Customers_Name",          // Provide the index name.
            //        e => e.Property(x => x.LastName),   // Specify at least one column.
            //        e => e.Property(x => x.FirstName))  // Multiple columns as desired.

            //    .HasIndex("IX_Customers_EmailAddress",  // Supports fluent chaining for more indexes.
            //        IndexOptions.Unique,                // Supports flags for unique and clustered.
            //        e => e.Property(x => x.EmailAddress));

            modelBuilder.Conventions.AddFromAssembly(this.GetType().Assembly);
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
        }
    }
}