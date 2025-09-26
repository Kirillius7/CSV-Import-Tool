using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TestApplicationPopovKyrylo.Models;

namespace TestApplicationPopovKyrylo.Services
{
    public class PersonSrvc : IPersonService
    {
        private readonly PersonDbContext _personDbContext;
        private readonly csvService _csvService;
        public PersonSrvc(PersonDbContext personDbContext, csvService csvService)
        {
            _personDbContext = personDbContext;
            _csvService = csvService;
        }

        public async Task <IEnumerable<Person>> Upload()
        {
            return await _personDbContext.people.ToListAsync();
        }

        public async Task DeleteRow(int? id)
        {
            if (id == null) return;

            var person = await _personDbContext.people.FindAsync(id.Value);
            if (person != null)
            {
                _personDbContext.people.Remove(person);
                await _personDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateRow(int id, [FromBody] PersonUpdateDTO updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            // Знайти користувача за його ID
            var person = await _personDbContext.people.FindAsync(id);
            if (person == null)
                throw new KeyNotFoundException($"Person with id {id} not found.");

            // Оновити дані
            person.Name = updated.Name;
            person.Phone = updated.Phone;

            //парсинг даних
            if (DateTime.TryParse(updated.DateOfBirth, out var dob))
                person.DateOfBirth = dob;

            if (bool.TryParse(updated.Married, out var married))
                person.Married = married;

            if (decimal.TryParse(updated.Salary, out var salary))
                person.Salary = salary;

            await _personDbContext.SaveChangesAsync();
        }
        public async Task<string> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Please choose a file to upload.";

            try
            {
                using var stream = file.OpenReadStream();
                await _csvService.ImportCsvAsync(stream);
                return null; 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
