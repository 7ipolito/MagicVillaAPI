using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.LoggerConfiguration;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
builder.WebHost.UseUrls($"http://*:{port}");

Log.Logger =  new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day).CreateLogger();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

    // Substitua os placeholders pelos valores das variáveis de ambiente
    connectionString = connectionString.Replace("POSTGRES_USER", postgresUser)
                                        .Replace("POSTGRES_PASSWORD", postgresPassword);

    options.UseNpgsql(connectionString);
});

// builder.Host.UseSerilog();
// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, LoggingV2>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
