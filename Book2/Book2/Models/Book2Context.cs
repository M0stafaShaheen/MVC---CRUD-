using Microsoft.EntityFrameworkCore;

namespace Book2.Models
{
    public class Book2Context:DbContext
    {

        public virtual DbSet<Book>Books {get; set;}

        public virtual DbSet<Auther>Authers { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog =Book2;Integrated Security = True ;TrustServerCertificate=True; ");
        }



    }
}
