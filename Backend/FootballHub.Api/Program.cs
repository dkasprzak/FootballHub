using FootballHub.Api.Auth;
using FootballHub.Api.Middlewares;
using FootballHub.Application;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Infrastructure;
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
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddJwtAuthenticationDataProvider(builder.Configuration);

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining(typeof(BaseCommandHandler));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

var app = builder.Build();

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
