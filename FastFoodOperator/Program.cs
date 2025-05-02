using FastFoodOperator.Model;
using FastFoodOperator.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace FastFoodOperator;

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
        app.UseWebSockets(); // Aktivera WebSocket-stöd

        var webSocketConnections = new List<WebSocket>();

        app.Map("/ws/orders", async (HttpContext context) =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                webSocketConnections.Add(webSocket);
                Console.WriteLine("WebSocket connected");

                var buffer = new byte[1024 * 4];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        webSocketConnections.Remove(webSocket);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        Console.WriteLine("WebSocket closed");
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        });

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

        app.MapEndpoints(webSocketConnections); // Skicka med WebSocket-listan

        app.Run();
    }
}
