using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.LoggerConfiguration;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);



Log.Logger =  new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day).CreateLogger();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// builder.Host.UseSerilog();
// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, LoggingV2>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Map controllers automatically

app.Run();
