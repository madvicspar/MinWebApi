using MinWebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace MinWebApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public float Rating { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }


public static class MovieEndpoints
{
	public static void MapMovieEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Movie").WithTags(nameof(Movie));

        group.MapGet("/", async (ApplicationContext db) =>
        {
            return await db.Movies.ToListAsync();
        })
        .WithName("GetAllMovies")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Movie>, NotFound>> (int id, ApplicationContext db) =>
        {
            return await db.Movies.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Movie model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetMovieById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Movie movie, ApplicationContext db) =>
        {
            var affected = await db.Movies
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, movie.Id)
                  .SetProperty(m => m.Title, movie.Title)
                  .SetProperty(m => m.Year, movie.Year)
                  .SetProperty(m => m.Rating, movie.Rating)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateMovie")
        .WithOpenApi();

        group.MapPost("/", async (Movie movie, ApplicationContext db) =>
        {
            db.Movies.Add(movie);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Movie/{movie.Id}",movie);
        })
        .WithName("CreateMovie")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationContext db) =>
        {
            var affected = await db.Movies
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteMovie")
        .WithOpenApi();
    }
}}
