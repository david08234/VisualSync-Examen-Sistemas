using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Base de datos: PostgreSQL (Supabase) con Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controladores y JSON
builder.Services.AddControllers().AddJsonOptions(o =>
{
    // Evita errores de "ciclo de objetos" entre Reserva, Usuario, Paquete, etc.
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // Los enums salen como texto (Solicitada, Aprobado...) y no como número
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// CORS: permite que el frontend de React llame a la API
builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
    p.WithOrigins("http://localhost:5173")
     .AllowAnyHeader()
     .AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");   // siempre antes de la autorización
app.UseAuthorization();
app.MapControllers();

app.Run();