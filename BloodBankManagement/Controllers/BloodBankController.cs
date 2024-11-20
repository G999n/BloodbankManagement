using System.Collections.Generic;
using System.Linq;
using BloodBankManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodBankController : ControllerBase
    {
        private static List<BloodBankEntry> _entries = new();
        private static int _nextId = 1;

        [HttpPost]
        public IActionResult Create(BloodBankEntry entry)
        {
            entry.Id = _nextId++;
            _entries.Add(entry);
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? size)
        {
            var result = _entries.AsQueryable();
            if (page.HasValue && size.HasValue)
            {
                if (page < 1 || size < 1)
                {
                    return BadRequest("Page and size must be greater than 0.");
                }

                int totalEntries = _entries.Count;
                int totalPages = (int)System.Math.Ceiling((double)totalEntries / size.Value);

                var paginatedResult = result.Skip((page.Value - 1) * size.Value).Take(size.Value).ToList();

                var response = new
                {
                    TotalEntries = totalEntries,
                    PageNumber = page.Value,
                    PageSize = size.Value,
                    TotalPages = totalPages,
                    Data = paginatedResult
                };

                return Ok(response);
            }
            return Ok(result.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, BloodBankEntry updatedEntry)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry == null) return NotFound();
            entry.DonorName = updatedEntry.DonorName;
            entry.Age = updatedEntry.Age;
            entry.BloodType = updatedEntry.BloodType;
            entry.ContactInfo = updatedEntry.ContactInfo;
            entry.Quantity = updatedEntry.Quantity;
            entry.CollectionDate = updatedEntry.CollectionDate;
            entry.ExpirationDate = updatedEntry.ExpirationDate;
            entry.Status = updatedEntry.Status;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry == null) return NotFound();
            _entries.Remove(entry);
            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? bloodType, [FromQuery] string? status, [FromQuery] string? donorName)
        {
            var result = _entries.AsQueryable();

            if (!string.IsNullOrEmpty(bloodType))
                result = result.Where(e => e.BloodType.ToLower() == bloodType.ToLower());

            if (!string.IsNullOrEmpty(status))
                result = result.Where(e => e.Status.ToLower() == status.ToLower());

            if (!string.IsNullOrEmpty(donorName))
                result = result.Where(e => e.DonorName.ToLower().Contains(donorName.ToLower()));

            return Ok(result.ToList());
        }
    }
}
