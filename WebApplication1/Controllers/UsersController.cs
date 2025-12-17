using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        // GET: /Users
        [HttpGet]
        public IActionResult GetAll()
        {
            using var context = new IDSDatabaseDbContext();
            return Ok(context.Users.ToList());
        }

        // GET: /Users/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using var context = new IDSDatabaseDbContext();

            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: /Users
        [HttpPost]
        public IActionResult Insert([FromBody] User user)
        {
            using var context = new IDSDatabaseDbContext();

            // Optional: if ID is manually provided, check for duplicates
            if (user.Id != 0 && context.Users.Any(u => u.Id == user.Id))
                return BadRequest("This ID already exists. Please choose a different one.");

            // If ID is 0, database will auto-generate it (if set as Identity)
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            context.Users.Add(user);
            context.SaveChanges();

            // Return 201 Created with the new user
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }



        // PUT: /Users/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User updatedUser)
        {
            using var context = new IDSDatabaseDbContext();

            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            // Update fields (adjust based on your User model)
            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.HashedPassword = updatedUser.HashedPassword;
            user.Role = updatedUser.Role;
            user.Status = updatedUser.Status;
            user.CreatedAt = updatedUser.CreatedAt;
            user.UpdatedAt = updatedUser.UpdatedAt;

            context.SaveChanges();

            return Ok(user);
        }

        // DELETE: /Users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var context = new IDSDatabaseDbContext();

            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok("User deleted successfully");
        }
    }
}
