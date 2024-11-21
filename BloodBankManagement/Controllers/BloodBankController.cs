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
        private static List<BloodBankEntry> _entries = new()
        {
            new BloodBankEntry
            {
                Id = 1,
                DonorName = "John Doe",
                Age = 30,
                BloodType = "A+",
                ContactInfo = "john.doe@example.com",
                Quantity = 500,
                CollectionDate = DateTime.Now.AddDays(-10),
                ExpirationDate = DateTime.Now.AddDays(20),
                Status = "Available"
            },
            new BloodBankEntry
            {
                Id = 2,
                DonorName = "Jane Smith",
                Age = 25,
                BloodType = "O+",
                ContactInfo = "jane.smith@example.com",
                Quantity = 300,
                CollectionDate = DateTime.Now.AddDays(-5),
                ExpirationDate = DateTime.Now.AddDays(25),
                Status = "Available"
            },
            new BloodBankEntry
            {
                Id = 3,
                DonorName = "Alex Johnson",
                Age = 40,
                BloodType = "B-",
                ContactInfo = "alex.johnson@example.com",
                Quantity = 450,
                CollectionDate = DateTime.Now.AddDays(-15),
                ExpirationDate = DateTime.Now.AddDays(15),
                Status = "Requested"
            },
            new BloodBankEntry
            {
                Id = 4,
                DonorName = "Emily Davis",
                Age = 35,
                BloodType = "AB+",
                ContactInfo = "emily.davis@example.com",
                Quantity = 600,
                CollectionDate = DateTime.Now.AddDays(-20),
                ExpirationDate = DateTime.Now.AddDays(10),
                Status = "Expired"
            },
            new BloodBankEntry
            {
                Id = 5,
                DonorName = "Michael Brown",
                Age = 29,
                BloodType = "A-",
                ContactInfo = "michael.brown@example.com",
                Quantity = 400,
                CollectionDate = DateTime.Now.AddDays(-7),
                ExpirationDate = DateTime.Now.AddDays(23),
                Status = "Available"
            },
            new BloodBankEntry
            {
                Id = 6,
                DonorName = "Sarah Wilson",
                Age = 50,
                BloodType = "O-",
                ContactInfo = "sarah.wilson@example.com",
                Quantity = 700,
                CollectionDate = DateTime.Now.AddDays(-3),
                ExpirationDate = DateTime.Now.AddDays(27),
                Status = "Available"
            },
            new BloodBankEntry
            {
                Id = 7,
                DonorName = "Chris Martin",
                Age = 34,
                BloodType = "B+",
                ContactInfo = "chris.martin@example.com",
                Quantity = 350,
                CollectionDate = DateTime.Now.AddDays(-8),
                ExpirationDate = DateTime.Now.AddDays(22),
                Status = "Requested"
            },
            new BloodBankEntry
            {
                Id = 8,
                DonorName = "Laura Garcia",
                Age = 28,
                BloodType = "AB-",
                ContactInfo = "laura.garcia@example.com",
                Quantity = 480,
                CollectionDate = DateTime.Now.AddDays(-12),
                ExpirationDate = DateTime.Now.AddDays(18),
                Status = "Available"
            },
            new BloodBankEntry
            {
                Id = 9,
                DonorName = "Daniel Lee",
                Age = 45,
                BloodType = "A+",
                ContactInfo = "daniel.lee@example.com",
                Quantity = 550,
                CollectionDate = DateTime.Now.AddDays(-9),
                ExpirationDate = DateTime.Now.AddDays(21),
                Status = "Requested"
            },
            new BloodBankEntry
            {
                Id = 10,
                DonorName = "Sophia Martinez",
                Age = 32,
                BloodType = "O+",
                ContactInfo = "sophia.martinez@example.com",
                Quantity = 300,
                CollectionDate = DateTime.Now.AddDays(-4),
                ExpirationDate = DateTime.Now.AddDays(26),
                Status = "Available"
            }
        };

        private static int _nextId = 11;

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