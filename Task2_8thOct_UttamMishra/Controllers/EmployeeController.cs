using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task2_8thOct_UttamMishra.Models;

namespace Task2_8thOct_UttamMishra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private static List<Models.Employee> employees = new List<Models.Employee>
        {
               new Employee { Id = 1, Name = "Uttam Mishra", Department = "IT", MobileNo = "1234567890", Email = "mishrauttam@gmail.com" },
               new Employee { Id = 2, Name = "Rohan Phadtare", Department = "IT", MobileNo = "1233567890", Email = "rohanphadtare@gmail.com" },
               new Employee { Id = 3, Name = "Mohit Bagul", Department = "IT", MobileNo = "1234566890", Email = "mohitbagul@gmail.com" },

        };

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById([FromRoute] int id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }
            return Ok(emp);
        }


        [HttpGet("bydept")]
        public ActionResult<IEnumerable<Employee>> GetEmployeesByDept([FromQuery] string department)
        {
            if (string.IsNullOrWhiteSpace(department)) 
                return BadRequest("department query is required.");
            var list = employees.Where(e => string.Equals(e.Department, department, System.StringComparison.OrdinalIgnoreCase));
            return Ok(list);
        }


        [HttpPost]
        public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
        {
            if (employees.Any(e => e.Id == employee.Id))
                return Conflict("Employee with Id " +employee.Id+" already exists.");

            employees.Add(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }


        [HttpPut("{id}")]
        public ActionResult UpdateEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (id != employee.Id) 
                return BadRequest("Id in route and body must match.");

            var existing = employees.FirstOrDefault(e => e.Id == id);
            if (existing == null) 
                return NotFound();
            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.MobileNo = employee.MobileNo;
            existing.Email = employee.Email;

            return NoContent();
        }

        [HttpPatch("{id}/email")]
        public ActionResult UpdateEmployeeEmail([FromRoute] int id, [FromBody] UpdateEmailDto dto)
        {
            var existing = employees.FirstOrDefault(e => e.Id == id);
            if (existing == null) 
                return NotFound();

            existing.Email = dto.Email;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee([FromRoute] int id)
        {
            var existing = employees.FirstOrDefault(e => e.Id == id);
            if (existing == null) return NotFound();

            employees.Remove(existing);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Append("Allow", "GET,POST,PUT,PATCH,DELETE,HEAD,OPTIONS");
            return Ok();
        }

    }
}
