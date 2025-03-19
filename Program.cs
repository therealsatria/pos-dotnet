using Microsoft.EntityFrameworkCore;
using Infrastructures.Data;
using Infrastructures.Repositories;
using Infrastructures.Services;
using Infrastructures.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => {
    options.Filters.Add<ValidationFilter>();
})
.ConfigureApiBehaviorOptions(options => {
    // Disable automatic model state validation
    options.SuppressModelStateInvalidFilter = true;
})
.AddJsonOptions(options => {
    // Handle reference cycles
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Dependency Injection for Repositories and Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// Configure CORS (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use global exception handling middleware
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Use CORS policy

app.UseAuthorization();

app.MapControllers();

// Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Apply migrations on startup
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();