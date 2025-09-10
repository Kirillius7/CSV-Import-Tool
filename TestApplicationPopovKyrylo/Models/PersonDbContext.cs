using Microsoft.EntityFrameworkCore;

namespace TestApplicationPopovKyrylo.Models
{
    public class PersonDbContext : DbContext
    {
        public DbSet<Person> people { get; set; }

        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options){}
    }
}
