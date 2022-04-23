using Cards.API.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//inject dbcontext

builder.Services.AddDbContext<CardsDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CardsDbConnectionString")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAny",
        x =>
        {
            x.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAny");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
