using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;
using WebApplication1.Repository.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetAll()
        {
            using var context = new IDSDatabaseDbContext();
            return Ok(context.Users.ToList());
        }

        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using var context = new IDSDatabaseDbContext();

            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        
        [HttpPost]
        public IActionResult Insert([FromBody] User user)
        {
            using var context = new IDSDatabaseDbContext();

           
            if (user.Id != 0 && context.Users.Any(u => u.Id == user.Id))
                return BadRequest("This ID already exists. Please choose a different one.");

            
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            context.Users.Add(user);
            context.SaveChanges();

       
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }



        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User updatedUser)
        {
            using var context = new IDSDatabaseDbContext();

            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            
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
