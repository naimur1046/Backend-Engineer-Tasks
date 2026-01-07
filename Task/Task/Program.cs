using Controllers.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

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

app.UseAuthorization();

app.MapControllers();

app.MapGet("/error", () => Results.Problem("An error occurred processing your request."))
   .ExcludeFromDescription();

app.Run();