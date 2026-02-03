using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Domain.Service;
using PasswordManager.Model.Builder;
using PasswordManager.Model.ViewModel;
using System.Security.Claims;

namespace PasswordManager.Web.Client.Controllers
{
    [Authorize]
    [ApiController]
    [Route("entries")]
    public class EntryController : Controller
    {
        private readonly IEntryService _entryService;
        private readonly IEntryModelBuilder _entryModelBuilder;

        public EntryController(IEntryService entryService, IEntryModelBuilder entryModelBuilder)
        {
            _entryService = entryService;
            _entryModelBuilder = entryModelBuilder;
        }

        // GET /entries
        [HttpGet]
        public async Task<IActionResult> GetAllEntries()
        {
            var userId = Convert.ToInt64(HttpContext.User.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            var entries = await _entryService.GetManyByUserAsync(userId);
            var entriesViewModel = _entryModelBuilder.Build(entries);
            return Ok(new { total = entries.Count, entries = entriesViewModel });
        }

        // GET /entries/{entryId}
        [HttpGet("{entryId:long}")]
        public async Task<IActionResult> GetByIdAsync(long entryId)
        {
            var entry = await _entryService.GetByIdAsync(entryId);
            if (entry == null)
            {
                return NotFound();
            }
            var entryViewModel = _entryModelBuilder.Build(entry);
            return Ok(entryViewModel);
        }

        // POST /entries
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EntryViewModel entryViewModel)
        {
            if (entryViewModel == null)
            {
                return BadRequest(new { message = "Entry data is required." });
            }
            var entry = _entryModelBuilder.Build(entryViewModel);
            await _entryService.CreateAsync(entry);
            if (!entry.ValidationResult.IsValid)
            {
                var errors = entry.ValidationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Failed to update the entry.", errors });
            }
            return Ok();
        }

        // PUT /entries/{entryId}        
        [HttpPut("{entryId:long}")]
        public async Task<IActionResult> UpdateAsync(long entryId, [FromBody] EntryViewModel entryViewModel)
        {
            if (entryViewModel == null)
            {
                return BadRequest(new { message = "Entry data is required." });
            }
            var existingEntry = await _entryService.GetByIdAsync(entryId);
            if (existingEntry == null)  
            {
                return NotFound();
            }
            var entry = _entryModelBuilder.Build(entryViewModel);
            entry.Id = entryId;
            await _entryService.UpdateAsync(entry);
            if (!entry.ValidationResult.IsValid)
            {
                var errors = entry.ValidationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Failed to update the entry.", errors });
            }
            return Ok();
        }

        // DELETE /entries/{entryId}
        [HttpDelete("{entryId:long}")]
        public async Task<IActionResult> DeleteAsync(long entryId)
        {
            var result = await _entryService.DeleteAsync(entryId);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
