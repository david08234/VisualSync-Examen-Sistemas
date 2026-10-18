using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar los controladores
builder.Services.AddControllers();

// 2. Configuración de Swagger (Asegúrate de tener estas dos líneas)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// (Aquí debe estar tu configuración de Entity Framework y Supabase...)

var app = builder.Build();

// 3. Activar la interfaz de Swagger (Asegúrate de tener este bloque)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();