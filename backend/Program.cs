
using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<PetBlogContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 22))));

builder.Services.AddScoped<IPetPostRepository, PetPostRepository>();

// Allow CORS
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader().AllowAnyMethod();
        });
    });

// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(policy =>
//     {
//         policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
//             .AllowAnyHeader()
//             .AllowAnyOrigin(); // For localhost only. Allow all
//     });
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
