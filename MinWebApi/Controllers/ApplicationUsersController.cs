using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Constructor for the ApplicationUsersController
        /// </summary>
        /// <param name="context">The database context</param>
        public ApplicationUsersController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>A list of users</returns>
        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
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
            var applicationUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            return applicationUser == null ? NotFound() : applicationUser;
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
            _context.Users.Add(applicationUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }
    }
}
