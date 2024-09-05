using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication1.Db;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private MSSQLContext _context;

        public DoctorsController(MSSQLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorListDto>>> GetDoctors(string sortBy = "FullName", int page = 1, int pageSize = 10)
        {
            var doctors = await _context.Doctors
                .Include(d => d.Cabinet)
                .Include(d => d.Specialization)
                .OrderBy(d => EF.Property<object>(d, sortBy))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DoctorListDto
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    CabinetName = d.Cabinet.Number,
                    Specialization = d.Specialization.Name

                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorListDto>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if(doctor == null)
                return NotFound();

            return Ok(new
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                CabinetName = doctor.Cabinet!.Number,
                Specialization = doctor.Specialization!.Name

            });
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var area = await _context.Areas.FindAsync(doctor.AreaId);
            var specialization = await _context.Specializations.FindAsync(doctor.SpecializationId);
            var cabinet = await _context.Cabinets.FindAsync(doctor.CabinetId);

            if(area != null)
                doctor.Area = area;
            if(specialization != null)
                doctor.Specialization = specialization;
            if(cabinet != null) 
                doctor.Cabinet = cabinet;

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id || !ModelState.IsValid)
                return BadRequest();

            var area = await _context.Areas.FindAsync(doctor.AreaId);
            var specialization = await _context.Specializations.FindAsync(doctor.SpecializationId);
            var cabinet = await _context.Cabinets.FindAsync(doctor.CabinetId);

            if (area != null)
                doctor.Area = area;
            if (specialization != null)
                doctor.Specialization = specialization;
            if (cabinet != null)
                doctor.Cabinet = cabinet;

            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok(doctor);
        }
    }
}
