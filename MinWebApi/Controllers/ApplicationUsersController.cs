using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>A list of users</returns>
        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                return await dataBase.Users.ToListAsync();
            }
        }

        /// <summary>
        /// Retrieves a specific user by ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>The specified application user / "NotFound" result</returns>
        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(int id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                var applicationUser = await dataBase.Users.FirstOrDefaultAsync(m => m.Id == id);
                return applicationUser == null ? NotFound() : applicationUser;
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="applicationUser">The application user to add</param>
        /// <returns>The created application user</returns>
        // POST: api/ApplicationUsers
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                dataBase.Users.Add(applicationUser);
                await dataBase.SaveChangesAsync();
                return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
            }
        }
    }
}
