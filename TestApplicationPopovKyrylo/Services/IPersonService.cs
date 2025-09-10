using Microsoft.AspNetCore.Mvc;
using TestApplicationPopovKyrylo.Models;

namespace TestApplicationPopovKyrylo.Services
{
    public interface IPersonService
    {
        IEnumerable<Person> Upload();
        Task DeleteRow(int? id);
        Task UpdateRow(int id, [FromBody] PersonUpdateDTO updated);
        Task<string> UploadCsv(IFormFile file);
    }
}
