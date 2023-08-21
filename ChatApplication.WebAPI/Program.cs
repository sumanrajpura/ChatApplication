using ChatApplication.Persistence;
using ChatApplication.Application;
using ChatApplication.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ChatApplication.Worker;
using ChatApplication.WebAPI.Middleware;
using ChatApplication.Application.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.AddHostedService<ChatQueueService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.

//Exception handling middleware.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var agentservice = services.GetRequiredService<IAgentService>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, agentservice);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured in migration");
}

app.Run();