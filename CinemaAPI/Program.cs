using CinemaAPI.Models;
using CinemaAPI.Respositories;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CinemaDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Description = "Authorization with JwT Token",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Id = "Bearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                }
            }, 
            new List<string>()
        }
    });
});

//Register
builder.Services.AddAutoMapper(typeof(Program));

//Life cycle DI
builder.Services.AddScoped<ICategoryMovieRespository, CategoryMovieRespository>();
builder.Services.AddScoped<ICategorySeatRespository, CategorySeatRespository>();
builder.Services.AddScoped<ISeatRespository, SeatRespository>();
builder.Services.AddScoped<IBillRespository, BillRespository>();
builder.Services.AddScoped<ICategoryMovie_MovieRespository, CategoryMovie_MovieRespository>();
builder.Services.AddScoped<IRoomRespository, RoomRespository>();
builder.Services.AddScoped<ICinemaRespository, CinemaRespository>();
builder.Services.AddScoped<IMovieRespository, MovieRespository>();
builder.Services.AddScoped<IShowTimeRespository, ShowTimeRespository>();
builder.Services.AddScoped<ITicketRespository, TicketRespository>();
builder.Services.AddScoped<IShiftRespository, ShiftRespository>();
builder.Services.AddScoped<INewRespository, NewRespository>();
builder.Services.AddScoped<IAccountRespository,AccountRespository>();
builder.Services.AddScoped<IRoleRespository, RoleRespository>();

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
app.UseAuthentication();
app.MapControllers();

app.Run();
