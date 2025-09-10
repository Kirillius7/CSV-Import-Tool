using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Formats.Asn1;
using System.Globalization;
using TestApplicationPopovKyrylo.Models;

namespace TestApplicationPopovKyrylo.Services
{
    public class csvService
    {
        private readonly PersonDbContext _context;

        public csvService(PersonDbContext context)
        {
            _context = context;
        }

        public async Task ImportCsvAsync(Stream fileStream)
        {
            var requiredHeaders = new[] { "Name", "DateOfBirth", "Married", "Phone", "Salary" };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                BadDataFound = null,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(fileStream); // зчитування даних з потока
            using var csv = new CsvReader(reader, config); // зчитування даних з файлу .csv

            // зчитування імен стовпців для подальшого аналізу та валідації
            await csv.ReadAsync();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            // перевірка на відсутність необхідних стовпців
            var missingHeaders = requiredHeaders.Except(headers, StringComparer.OrdinalIgnoreCase).ToList();
            if (missingHeaders.Any())
            {
                throw new Exception($"CSV file is missing required columns: {string.Join(", ", missingHeaders)}");
            }

            var records = csv.GetRecords<Person>().ToList();

            foreach (var person in records)
            {
                if (string.IsNullOrWhiteSpace(person.Name))
                    throw new Exception("Name is required");
                if (person.Salary < 0)
                    throw new Exception("Salary must be positive");
            }

            _context.people.AddRange(records);
            await _context.SaveChangesAsync();
        }
    }
}
