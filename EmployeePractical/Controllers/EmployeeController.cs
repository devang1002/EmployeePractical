using EmployeePractical.Context;
using EmployeePractical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeePractical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly MyEmployeeContext _context;
        public EmployeeController(MyEmployeeContext context)
        {
            (_context) = (context);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<Employee>> Add([FromForm]Employee employee)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHssfff") + "_" + Path.GetFileName(employee.ImageUrl.FileName);
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Employee");

            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await employee.ImageUrl.CopyToAsync(fileStream);
            employee.Image = "Employee" + fileName;

            _context.GetEmployees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult<Employee>> Update([FromForm]Employee employee)
        {
            var result = await _context.GetEmployees.FindAsync(employee.Id);
            if (result != null)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHssfff") + "_" + Path.GetFileName(employee.ImageUrl.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Employee");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var filePath = Path.Combine(directoryPath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await employee.ImageUrl.CopyToAsync(fileStream);
                employee.Image = "Employee" + fileName;

                result.Name = employee.Name;
                result.StartingDate = employee.StartingDate;
                result.MaritalStatus = employee.MaritalStatus;
                result.DOB = employee.DOB;
                result.Gender = employee.Gender;
                result.Email = employee.Email;
                result.Mobile = employee.Mobile;

                _context.GetEmployees.Update(result);
                await _context.SaveChangesAsync();
                return Ok(employee);
            }
            else
            {
                return BadRequest("Employee Not Update.");
            }
        }

        [HttpGet("GetAllEmployeeList")]
        public async Task<ActionResult<Employee>> GetAllEmployee()
        {
            var result = await _context.GetEmployees.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByEmployeeId(Guid id)
        {
            var result = await _context.GetEmployees.FirstOrDefaultAsync(x=> x.Id == id);
            if(result == null)
            {
                return BadRequest("No Data.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var result = await _context.GetEmployees.FirstOrDefaultAsync(x=> x.Id == id);
            if(result == null)
            {
                return BadRequest("Not Deleted");
            }
            _context.GetEmployees.Remove(result);
            await _context.SaveChangesAsync();
            return Ok("Delete Done");
        }

        [HttpGet("FindByGender")]
        public async Task<IActionResult> FindByGender(string gender)
        {
            var result = await _context.GetEmployees.Where(x => x.Gender == gender).ToListAsync();
            return Ok(result);
        }

        [HttpGet("FindEmployeeByGender")]
        public async Task<IActionResult> FindEmployeeByGender()
        {
            var query = $@"select Count(case when Gender = 'Male' then 1 END) as MaleCount,
		                    count(case when Gender = 'Female' then 1 END) as FemaleCount,
		                    count(*) as AllEmployee		
		                    from GetEmployees";
            var result = await _context.Database.ExecuteSqlRawAsync(query);
            return Ok(result);
            // var result = await _context.GetEmployees.Where(x => x.Gender == "Male").ToListAsync();
            //return Ok(new {result.Count});
        }
    }
}
