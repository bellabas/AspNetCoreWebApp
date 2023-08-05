using AspNetCoreWebApp.ApiEndpoint.Database;
using AspNetCoreWebApp.ApiEndpoint.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.ApiEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            builder.Services.AddDbContext<UserDbContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // ReadAll
            app.MapGet("/users", async (UserDbContext db) => await db.Users.ToListAsync());
            // Read
            app.MapGet("/users/{id}", async (UserDbContext db, int id) => await db.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound());
            // Create
            app.MapPost("/users", async (UserDbContext db, User user) =>
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/users/{user.Id}", user);
            });
            // Update
            app.MapPut("/users/{id}", async (UserDbContext db, int id, User user) =>
            {
                var userToUpdate = await db.Users.FindAsync(id);

                if (userToUpdate is null)
                {
                    return Results.NotFound();
                }

                userToUpdate.Username = user.Username;
                userToUpdate.Password = user.Password;

                await db.SaveChangesAsync();

                return Results.Ok(user);
            });
            // Delete
            app.MapDelete("/users/{id}", async (UserDbContext db, int id) =>
            {
                if (await db.Users.FindAsync(id) is User userToDelete)
                {
                    db.Users.Remove(userToDelete);
                    await db.SaveChangesAsync();
                    return Results.Ok(userToDelete);
                }
                return Results.NotFound();
            });

            app.Run();
        }
    }
}