using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VisualSync.Api.Data;
using VisualSync.Api.Models;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Configuración obligatoria
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta la configuración Jwt:Key.");
if (jwtKey.Length < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 caracteres.");

var corsOrigen = builder.Configuration["Cors:Origen"] ?? "http://localhost:5173";

// Base de datos: PostgreSQL (Supabase) con Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controladores y JSON
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// CORS
builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
    p.WithOrigins(corsOrigen).AllowAnyHeader().AllowAnyMethod()));

// Autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "VisualSync.Api",
        ValidateAudience = true,
        ValidAudience = "VisualSync.Web",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = ClaimTypes.Role
    });
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración para el proxy de producción (Punto 12)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// app.UseMiddleware<ExceptionMiddleware>(); // descomenta si ya creaste el middleware de errores

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Administrador inicial
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var adminEmail = app.Configuration["Seed:AdminEmail"];
    var adminPassword = app.Configuration["Seed:AdminPassword"];

    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword)
        && !db.Usuarios.Any(u => u.Rol == "Admin"))
    {
        db.Usuarios.Add(new Usuario
        {
            NombreCompleto = "Administrador del Sistema",
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword, 11),
            Rol = "Admin"
        });
        db.SaveChanges();
    }
}

app.Run();