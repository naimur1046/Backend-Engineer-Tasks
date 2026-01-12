using Controllers.Services;
using Microsoft.OpenApi.Models;
using Serilog;

var logsPath = Path.Combine(Directory.GetCurrentDirectory(), "__logs");
Directory.CreateDirectory(logsPath);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(logsPath, "app-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Backend Task API");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog for logging
    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    builder.Services.AddScoped<INumberService, NumberService>();
    builder.Services.AddScoped<ITemperatureService, TemperatureService>();
    builder.Services.AddScoped<IDateService, DateService>();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Backend Task API",
            Version = "v1",
            Description = "This is a take-home backend engineering assignment",
        });
    });

    builder.Services.AddHttpClient("default", client =>
    {
        client.Timeout = TimeSpan.FromSeconds(30);
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Task API v1");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "Backend Task API Documentation";
    });

    app.UseHttpsRedirection();

    app.UseCors();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.MapGet("/error", () => Results.Problem("An error occurred processing your request."))
        .ExcludeFromDescription();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}