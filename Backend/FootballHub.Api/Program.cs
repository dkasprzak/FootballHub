using System.Text.Json.Serialization;
using FootballHub.Api.Auth;
using FootballHub.Api.Middlewares;
using FootballHub.Application;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var APP_NAME = "FootballHub.Api";

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", APP_NAME)
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();
    
var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.local.json");
}

builder.Host.UseSerilog((context, services, configuration) => configuration
    .Enrich.WithProperty("Application", APP_NAME)
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(options =>
{
    if (!builder.Environment.IsDevelopment())
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }
}).AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddValidators();
builder.Services.AddJwtAuthenticationDataProvider(builder.Configuration);

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
});

builder.Services.AddSwaggerGen(o => 
    o.CustomSchemaIds(x =>
    {
        var name = x.FullName;
        if (name != null)
        {
            name = name.Replace("+", "_"); //swagger bug fix
        }
        return name;
    }));

builder.Services.AddAntiforgery(o =>
{
    o.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins(app.Configuration.GetValue<string>("WebAppBaseUrl") ?? "")
    .WithOrigins(app.Configuration.GetSection("AdditionalCorsOrigins").Get<string[]>() ?? new string[0])
    .WithOrigins((Environment.GetEnvironmentVariable("AdditionalCorsOrigins") ?? "").Split(",").Where(h => !string.IsNullOrEmpty(h)).Select(h => h.Trim()).ToArray())
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
);

app.UseExceptionResultMiddleware();

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
