using Microsoft.EntityFrameworkCore;
using MovieCatalogApi_laba3.Endpoints;
using MovieCatalogApi_laba3.Data;

namespace MovieCatalogApi_laba3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=movies.db");
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            app.MapDirectorEndpoints();
            app.MapGenreEndpoints();
            app.MapMovieEndpoints();

            app.Run();
        }
    }
}
