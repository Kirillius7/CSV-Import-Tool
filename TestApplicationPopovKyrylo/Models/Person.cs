using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApplicationPopovKyrylo.Models
{
    public class Person
    {
        [Key] // ключ таблиці
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // функція автоінкременту рядків по id
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Married { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }
    }
}
