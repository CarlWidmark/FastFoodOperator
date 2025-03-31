using FastFoodOperator.Model;
using FastFoodOperator.Services;
using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        // Allow only the frontend's origin to make requests
                        policy.WithOrigins("http://localhost:5173") // The URL where your Vue app is running
                              .AllowAnyMethod()  // Allow any HTTP method
                              .AllowAnyHeader(); // Allow any HTTP header
                    });
            });

            builder.Services.AddDbContext<PizzaShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PizzaShopDb")));

            var app = builder.Build();

            app.UseCors("AllowFrontend");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PizzaShopContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                DatabaseHelper.PopulateDatabase(db, scope.ServiceProvider);
            }
            app.MapEndpoints();



            app.Run();
        }
    }
}
