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
            builder.Services.AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase("User"));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Swagger and API
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HttpClient
            builder.Services.AddScoped<HttpClient>();
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
            app.MapGet("/api/users/{username}", async (UserDbContext db, string username) => await db.Users.FindAsync(username) is User user ? Results.Ok(user) : Results.NotFound());
            // Create
            app.MapPost("/api/users", async (UserDbContext db, User user) =>
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.Username}", user);
            });
            // Update
            app.MapPut("/api/users/{username}", async (UserDbContext db, User user) =>
            {
                var userToUpdate = await db.Users.FindAsync(user.Username);

                if (userToUpdate is null)
                {
                    return Results.NotFound();
                }

                userToUpdate.Password = user.Password;

                await db.SaveChangesAsync();

                return Results.Ok(userToUpdate);
            });
            // Delete
            app.MapDelete("/api/users/{username}", async (UserDbContext db, string username) =>
            {
                if (await db.Users.FindAsync(username) is User userToDelete)
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