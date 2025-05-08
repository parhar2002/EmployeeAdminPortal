using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EmployeesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = context.Employees.ToList();
            if (employees == null || !employees.Any())
            {
                return NotFound("No employees found.");
            }
            return Ok(employees);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] AddEmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = employeeDto.FirstName,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };
            context.Employees.Add(employee);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetAllEmployees), new { id = employee.Id }, employee);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] AddEmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            employee.FirstName = employeeDto.FirstName;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;
            employee.Salary = employeeDto.Salary;
            context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            context.Employees.Remove(employee);
            context.SaveChanges();
            return NoContent();
        }

    }
}
