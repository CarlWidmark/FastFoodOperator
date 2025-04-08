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
                        policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
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
                DatabaseHelper.PopulateDatabase(db);
            }
            app.MapEndpoints();

            app.MapPost("/receipt", (decimal totalPrice) =>
            {
                string receipt = TaxCalculator.FormatReceipt(totalPrice);
                return Results.Ok(receipt);
            });



            app.Run();
        }
    }
}
