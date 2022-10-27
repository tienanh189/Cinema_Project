using CinemaAPI.Models;
using CinemaAPI.Respositories;
using CinemaAPI.Respositories.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register
builder.Services.AddAutoMapper(typeof(Program));

//Life cycle DI
builder.Services.AddScoped<ICategoryMovieRespository, CategoryMovieRespository>();
builder.Services.AddScoped<ISeatRespository, SeatRespository>();
builder.Services.AddScoped<ICategorySeatRespository, CategorySeatRespository>();
builder.Services.AddScoped<IDiscountRespository, DiscountRespository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
