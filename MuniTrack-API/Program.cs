using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Domain.Enum;
using Npgsql; // Asegura la conexión con Postgres
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// --- SERVICIOS ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger (Requisito 50)
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("AgendaApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
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

// --- BASE DE DATOS POSTGRESQL ---
// Abstracción mediante Entity Framework (Requisito 46)
builder.Services.AddDbContext<MuniDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("MuniAPIDBConnectionString")));

// --- AUTENTICACIÓN JWT ---
builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Authentication:Issuer"],
           ValidAudience = builder.Configuration["Authentication:Audience"],
           IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"] ?? "ClaveSuperSecretaDeRespaldo32Chars")),
           RoleClaimType = "role" 
       };
   });

#region Inyección de Dependencias
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<ICitizenService, CitizenService>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<ICitizenRepository, CitizenRepository>();
builder.Services.AddScoped<IIncidenceService, IncidenceService>();
builder.Services.AddScoped<IIncidenceRepository, IncidenceRepository>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("MuniTrackPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

// --- ROLES (Requisito 24) ---
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlySysAdmin", policy => policy.RequireRole(Role.SysAdmin.ToString()));
    options.AddPolicy("AdminAndAbove", policy => policy.RequireRole(Role.SysAdmin.ToString(), Role.Admin.ToString()));
    options.AddPolicy("AnyRole", policy => policy.RequireRole(Role.SysAdmin.ToString(), Role.Admin.ToString(), Role.OperatorBasic.ToString()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MuniTrackPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();