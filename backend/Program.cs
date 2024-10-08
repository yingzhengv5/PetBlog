using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Repositories;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

// Get MySQL variables
var server = Environment.GetEnvironmentVariable("MYSQL_SERVER");
var database = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
var user = Environment.GetEnvironmentVariable("MYSQL_USER");
var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

// Build the connection string
var connectionString = $"Server={server};Database={database};User={user};Password={password};";

//Add services to the container
builder.Services.AddControllers();

// builder.Services.AddDbContext<PetBlogContext>(options =>
//     options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 22))));

builder.Services.AddDbContext<PetBlogContext>(options =>
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 22)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

builder.Services.AddScoped<IPetPostRepository, PetPostRepository>();

// Add Cloudinary configuration
builder.Services.AddSingleton(new Cloudinary(new Account(
    Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
    Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
)));

// Allow CORS
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Apply migrations at runtime
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PetBlogContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.MapControllers();

app.Run();