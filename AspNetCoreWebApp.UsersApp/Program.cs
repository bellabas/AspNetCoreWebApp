using AspNetCoreWebApp.UsersApp.Database;
using AspNetCoreWebApp.UsersApp.Models;
using AspNetCoreWebApp.UsersApp.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.UsersApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add DbContext
            builder.Services.AddDbContext<UserDbContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Swagger and API
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HttpClient
            builder.Services.AddHttpClient<ApiClientService>();
            builder.Services.AddScoped<ApiClientService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapSwagger());

            app.MapRazorPages();

            // MINIMAL API
            // ReadAll
            app.MapGet("/api/users", async (UserDbContext db) => await db.Users.ToListAsync());
            // Read
            app.MapGet("/api/users/{id}", async (UserDbContext db, int id) => await db.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound());
            // Create
            app.MapPost("/api/users", async (UserDbContext db, User user) =>
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.Id}", user);
            });
            // Update
            app.MapPut("/api/users/{id}", async (UserDbContext db, int id, User user) =>
            {
                var userToUpdate = await db.Users.FindAsync(id);

                if (userToUpdate is null)
                {
                    return Results.NotFound();
                }

                userToUpdate.Username = user.Username;
                userToUpdate.Password = user.Password;

                await db.SaveChangesAsync();

                return Results.Ok(userToUpdate);
            });
            // Delete
            app.MapDelete("/api/users/{id}", async (UserDbContext db, int id) =>
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