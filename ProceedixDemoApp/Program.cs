using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProceedixDemoApp.Data;
using ProceedixDemoApp.DTOs;
using ProceedixDemoApp.Repositories;
using ProceedixDemoApp.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register queue and background service
builder.Services
    .AddHostedService<LogMessageBackgroundService>()
    .AddSingleton<IBackgroundQueue<LogMessageDto[]>, BackgroundQueue<LogMessageDto[]>>();

// Register interfaces
builder.Services.AddScoped<ILogMessageRepository, LogMessageRepository>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

// Initialize data source for PostgreSQL and add it as context
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.MapEnum<ProceedixDemoApp.Models.LogLevel>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<ProceedixDemoAppDbContext>(options => options.UseNpgsql(dataSource));

var app = builder.Build();

// Automatic migration on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ProceedixDemoAppDbContext>();
    await context.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();