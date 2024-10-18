using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IServiceService,ServiceService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<PRM392_HomeBuddyContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PRM392_HomeBuddyContext>();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

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
