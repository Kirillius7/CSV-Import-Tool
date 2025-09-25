using Microsoft.EntityFrameworkCore;

namespace TestApplicationPopovKyrylo.Models
{
    public class PersonDbContext : DbContext // підключення до БД, робота з транзакціями, CRUD-операції
    {
        public DbSet<Person> people { get; set; } // оголошення таблиці (колекції сутностей)
        // для роботи із даними як із колекцією (а не таблицею і рядком SQL)

        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) { }
        // підключення до відповідної БД, врахування додаткового функціоналу (lazy loading, логування)
        // як поводитись із транзакціями та міграціями
    }
}
