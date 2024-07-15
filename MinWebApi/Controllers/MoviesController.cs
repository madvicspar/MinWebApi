using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinWebApi.Models;

namespace MinWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /// <summary>
        /// Retrieves all movies
        /// </summary>
        /// <returns>A list of movies</returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                return await dataBase.Movies.ToListAsync();
            }
        }

        /// <summary>
        /// Retrieves a specific movie by ID
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve</param>
        /// <returns>The specified movie</returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                var movie = await dataBase.Movies.FirstOrDefaultAsync(m => m.Id == id);
                return movie == null ? NotFound() : movie;
            }
        }

        /// <summary>
        /// Adds a new movie to the database
        /// </summary>
        /// <param name="movie">The movie to add</param>
        /// <returns>The created movie</returns>
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                if (dataBase == null)
                {
                    return StatusCode(500, "Database access failed.");
                }

                dataBase.Movies.Add(movie);
                await dataBase.SaveChangesAsync();
                return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
            }
        }
    }
}
