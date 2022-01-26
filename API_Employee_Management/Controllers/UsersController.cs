using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Employee_Management.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly dbEmployeeManagementContext _context;

        public UsersController(dbEmployeeManagementContext context)
        {
            _context = context;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(string credenciales)
        {

            if (credenciales == null)
            {
                return BadRequest("Invalid client request");
            }

            string[] valores = credenciales.Split(",");

            var login = valores[0];
            var pass = valores[1];

            var user = await _context.Users.Where(u => u.Login == login && u.Pass == pass).FirstOrDefaultAsync();
 

            if (user != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }






        // GET: api/Users/5
        [HttpGet("GetPermissionsByRole/{RoleId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermissionsByRole(int RoleId)
        {
            var per = await _context.RolePermissions.Include(r => r.Role)
                                                    .Include(p => p.Permission)
                                                    .Where(pr => pr.RoleId == RoleId)
                                                    .Select(s => s.Permission.Name).ToListAsync();

            if (per == null)
            {
                return NotFound();
            }

            return per;
        }


        // GET: api/Users/5
        [HttpGet("GetRoleByUser/{login};{pass}")]
        public async Task<ActionResult<int>> GetRoleByUser(string login, string pass)
        {
            var role = await _context.Users.Where(u => u.Login == login && u.Pass == pass).Select(s => s.RoleId).FirstOrDefaultAsync();

            if (role <= 0 )
            {
                return 0;
            }

            return role;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
