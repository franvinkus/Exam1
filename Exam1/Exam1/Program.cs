using Exam1.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ticket.Entities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextPool<Exam1Context>(options =>
{
    var constring = configuration.GetConnectionString("DbSqlServer");
    options.UseSqlServer(constring);
});

builder.Services.AddTransient<AvailTicketServices>();
builder.Services.AddTransient<BookedTicketServices>();
builder.Services.AddTransient<PdfGerenatorService>();

//Add Serilog to the project
builder.Host.UseSerilog((context, LoggerConfig) =>
{
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }

    LoggerConfig
    .MinimumLevel.Information()
    .WriteTo.File(
        path: Path.Combine(logDirectory,"log-.txt"),
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 10_000_000,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1)
        );
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
