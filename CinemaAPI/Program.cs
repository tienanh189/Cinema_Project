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
builder.Services.AddScoped<ICategorySeatRespository, CategorySeatRespository>();
builder.Services.AddScoped<ISeatRespository, SeatRespository>();
builder.Services.AddScoped<IDiscountRespository, DiscountRespository>();
builder.Services.AddScoped<IBillRespository, BillRespository>();
builder.Services.AddScoped<ICategoryMovie_MovieRespository, CategoryMovie_MovieRespository>();
builder.Services.AddScoped<IShowTime_SeatRespository, ShowTime_SeatRespository>();
builder.Services.AddScoped<IRoomRespository, RoomRespository>();
builder.Services.AddScoped<ICinemaRespository, CinemaRespository>();
builder.Services.AddScoped<IMovieRespository, MovieRespository>();
builder.Services.AddScoped<IShowTimeRespository, ShowTimeRespository>();
builder.Services.AddScoped<ITicketRespository, TicketRespository>();


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

app.Run();
