using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WellWork.Api.Extensions;
using WellWork.Application;
using WellWork.Application.Configs;
using WellWork.Application.Mappings;
using WellWork.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configs = builder.Configuration.Get<Settings>();
builder.Services.AddSingleton(configs);

// ----------------------------
//        CONFIGURAÇÃO
// ----------------------------

builder.Services.AddVersioning();
// Swagger
builder.Services.AddSwagger(configs);

// Controllers
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(CheckInMappingProfile), typeof(GeneratedMessageMappingProfile));

// Application Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<IGeneratedMessageService, GeneratedMessageService>();
builder.Services.AddHttpClient<ILLMService, GroqLLMService>();

// Infrastructure (repos, db, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthCheckConfiguration(builder.Configuration);

var app = builder.Build();

// ------------------------------
// Health Check Endpoints
// ------------------------------
app.MapHealthChecks("/health");
app.MapHealthChecks("/health-details", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json",  "WellWork.API v1");
        ui.SwaggerEndpoint("/swagger/v2/swagger.json",  "WellWork.API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();