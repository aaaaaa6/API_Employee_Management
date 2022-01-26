using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Employee_Management.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace API_Employee_Management.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly dbEmployeeManagementContext _context;

        public EmployeesController(dbEmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet("GetEmployees")]
     
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var response = await _context.Employees.OrderBy(o=> o.Nombre).ToListAsync();

            return Ok(response);
        }

        // GET: api/Employees/5
        [HttpGet("GetEmployeesBySearch")]
      
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesBySearch(string search = null)
        {
            if (search != null)
            {
                var employee = await _context.Employees.Where(e => e.Cedula.Contains(search) || e.Nombre.Contains(search)).OrderBy(o => o.Nombre).ToListAsync();

                if (employee == null)
                {
                    return NotFound();
                }

                return employee;
            }
            else
            {
                var employee = await _context.Employees.OrderBy(o => o.Nombre).ToListAsync();

                return Ok(employee);
            }   
            
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("PutEmployee")]
        public async Task<ActionResult<Employee>>PutEmployee([FromBody]Employee employee)
        {
            
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Cedula))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployees", new { id = employee.Id }, employee);
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("InsertEmployee")]
        public async Task<ActionResult<Employee>> InsertEmployee([FromBody]Employee employees)
        {
            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", new { id = employees.Id }, employees);
        }

        [HttpPost("InsertEmployees")]
        public async Task<ActionResult<List<Employee>>> InsertEmployees([FromBody] List<Employee> employees)
        {
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployees", employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(string cedula)
        {
            return _context.Employees.Any(e => e.Cedula == cedula);
        }
    }
}
