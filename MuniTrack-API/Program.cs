using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Domain.Enum;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.ExternalServices;
using Polly;
using Polly.Extensions.Http;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// 1. LIMPIEZA DE MAPEO DE CLAIMS (Para que el rol no se rompa)
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// --- SERVICIOS BASE ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 1. Evita el bucle infinito que causaba el error de ciclo
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        
        // 2. Fuerza a que el JSON use minúsculas al inicio (camelCase)
        // Esto asegura que tus grillas existentes NO se rompan.
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; 
    });
builder.Services.AddEndpointsApiExplorer();

// --- CONFIGURACIÓN SWAGGER ---
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("AgendaApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Pegar el token generado al loguearse."
    });
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "AgendaApiBearerAuth" }
            }, new List<string>() }
    });
});

// --- 2. BASE DE DATOS (ESTO ES LO QUE FALTABA Y HACÍA EXPLOTAR LA APP) ---
builder.Services.AddDbContext<MuniDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("MuniAPIDBConnectionString")));

// --- 3. AUTENTICACIÓN JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters()
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Authentication:Issuer"],
           ValidAudience = builder.Configuration["Authentication:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"] ?? "ClaveSuperSecretaDeRespaldo32Chars")),
           RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
       };

       options.Events = new JwtBearerEvents
       {
           OnTokenValidated = context =>
           {
               var claims = context.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
               Console.WriteLine("--- CLAIMS DETECTADAS EN EL TOKEN ---");
               foreach (var claim in claims) Console.WriteLine(claim);
               return Task.CompletedTask;
           },
           OnAuthenticationFailed = context => 
           {
               Console.WriteLine("--- ERROR DE AUTENTICACIÓN ---");
               Console.WriteLine(context.Exception.Message);
               return Task.CompletedTask;
           }
       };
   });

// --- 4. INYECCIÓN DE DEPENDENCIAS ---
#region Inyección de Dependencias
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICitizenService, CitizenService>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<ICitizenRepository, CitizenRepository>();
builder.Services.AddScoped<IIncidenceService, IncidenceService>();
builder.Services.AddScoped<IIncidenceRepository, IncidenceRepository>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IWeatherApiService, WeatherService>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepositoryService>();

#endregion

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("MuniTrackPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlySysAdmin", policy => policy.RequireRole(Role.SysAdmin.ToString()));
    options.AddPolicy("AdminAndAbove", policy => policy.RequireRole(Role.SysAdmin.ToString(), Role.Admin.ToString()));
    options.AddPolicy("AnyRole", policy => policy.RequireRole(Role.SysAdmin.ToString(), Role.Admin.ToString(), Role.OperatorBasic.ToString()));
});


var apiClientConfig = builder.Configuration.GetSection("ApiClientConfiguration").Get<ApiClientConfiguration>();
// 1. Definimos la política: ¿Cuándo reintentar?
// Esto permite manejar errores de red de forma automática [cite: 14, 24]
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // solo reintenta si ocurre un error transitorio
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Espera exponencial: 2s, 4s, 8s 

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 2, 
        durationOfBreak: TimeSpan.FromSeconds(30)
    );

// 2. Registramos el HttpClientFactory con el nombre "weatherHttpClient"
// Esto evita el agotamiento de sockets y mejora el rendimiento [cite: 187, 195, 201]
builder.Services.AddHttpClient("weatherHttpClient", client => 
{
  
    client.BaseAddress = new Uri("https://my.meteoblue.com/");
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseCors("MuniTrackPolicy");

app.UseAuthentication(); // Primero quién sos
app.UseAuthorization();  // Segundo qué podés hacer

app.MapControllers();
app.Run();