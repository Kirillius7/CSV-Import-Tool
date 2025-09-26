
using Microsoft.AspNetCore.Mvc;
using TestApplicationPopovKyrylo.Models;
using TestApplicationPopovKyrylo.Services;

public class PersonController : Controller
{
    private readonly PersonDbContext _context;
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService, PersonDbContext context)
    {
        _personService = personService;
        _context = context;
    }

    [HttpGet]
    public async Task <IActionResult> Upload()
    {
        var people = await _personService.Upload();
        return View("~/Views/Home/Upload.cshtml", people);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteRow(int? id)
    {
        await _personService.DeleteRow(id);
        return View("~/Views/Home/Upload.cshtml", _context.people);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRow(int id, [FromBody] PersonUpdateDTO updated)
    {
        try
        {
            await _personService.UpdateRow(id, updated);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadCsv(IFormFile file) // з <input type="file" name="file" ...>
    {
        var errorMessage = await _personService.UploadCsv(file);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ViewBag.ErrorMessage = errorMessage;
            return View("~/Views/Home/Upload.cshtml");
        }

        return RedirectToAction("Upload");
    }
}
