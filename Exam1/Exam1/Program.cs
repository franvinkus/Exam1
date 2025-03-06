using System.Reflection;
using Exam1.Services;
using Exam1.Validator;
using FluentValidation.AspNetCore;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ticket.Entities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

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

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());


builder.Services.AddValidatorsFromAssemblyContaining<GetAvailTicketValidator>();
builder.Services.AddFluentValidationAutoValidation();

//builder.Services.AddTransient<AvailTicketServices>();
//builder.Services.AddTransient<BookedTicketServices>();
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

app.UseCors("AllowAll");

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
