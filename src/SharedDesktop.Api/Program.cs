
using SharedDesktop.Api;
using SharedDesktop.Api.Hubs;
using SharedDesktop.Api.Services;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSignalR();
        builder.Services.AddSignalRCore();

        builder.Services.AddSingleton<INetworkService, NetworkService>();
        builder.Services.AddHostedService<DnsBackgrounService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapHub<SynchronizationHub>("/realtime/synchronization");

        app.MapGet("/ping", () => "Ok, let's go!");

        app.MapControllers();

        await app.RunAsync($"http://0.0.0.0:0");
    }
}