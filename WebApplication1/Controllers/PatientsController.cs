using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private MSSQLContext  _context;
        public PatientsController(MSSQLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientListDto>>> GetPatients(string sortBy = "LastName", int page = 1, int pageSize = 10)
        {
            var patients = await _context.Patients
                .OrderBy(p => EF.Property<object>(p, sortBy))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PatientListDto
                {
                    Id = p.Id,
                    FullName = $"{p.LastName} {p.FirstName} {p.MiddleName}",
                    Address = p.Address,
                    Gender = p.Gender
                })
                .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            return Ok(new
            {
                Id = patient.Id,
                FullName = $"{patient.LastName} {patient.FirstName} {patient.MiddleName}",
                Address = patient.Address,
                Gender = patient.Gender,
                AreaNumber = patient.Area.AreaNumber,
            });
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var area = await _context.Areas.FindAsync(patient.AreaId);

            if (area != null)
                patient.Area = area;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPatient(int id, Patient patient)
        {
            if (id != patient.Id || !ModelState.IsValid)
                return BadRequest();

            var area = await _context.Areas.FindAsync(patient.AreaId);

            if (area != null)
                patient.Area = area;

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok(patient);
        }

    }
}
