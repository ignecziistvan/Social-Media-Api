using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
  .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try {
  var context = services.GetRequiredService<DataContext>();
  var userManager = services.GetRequiredService<UserManager<User>>();
  var roleManager = services.GetRequiredService<RoleManager<Role>>();
  await context.Database.MigrateAsync();
  await Seed.SeedUsers(userManager, roleManager);
} catch (Exception e) {
  var logger = services.GetRequiredService<ILogger<Program>>();
  logger.LogError(e, "An error occoured during migration");
}

app.Run();
