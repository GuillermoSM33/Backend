using backend.Models; 
using Microsoft.EntityFrameworkCore;
/* using backend.Controllers; */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsuariosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader() 
                  .AllowAnyMethod(); 
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

//Esta parte del código es la que se encarga de manejar las peticiones HTTP sin necesidad de crear un controlador

/* // GET: Obtener todos los usuarios
app.MapGet("/usuarios", async (UsuariosContext db) =>
{
    return await db.Usuarios.ToListAsync();
});

// POST: Crear un nuevo usuario
app.MapPost("/usuarios", async (UsuariosContext db, Usuario nuevoUsuario) =>
{
    db.Usuarios.Add(nuevoUsuario);
    await db.SaveChangesAsync();
    return Results.Created($"/usuarios/{nuevoUsuario.Id}", nuevoUsuario);
});

// PUT: Actualizar un usuario existente
app.MapPut("/usuarios/{id}", async (int id, UsuariosContext db, Usuario usuarioActualizado) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario == null) return Results.NotFound();

    usuario.Nombre = usuarioActualizado.Nombre;
    usuario.Edad = usuarioActualizado.Edad;
    usuario.EsEstudiante = usuarioActualizado.EsEstudiante;
    usuario.Direccion = usuarioActualizado.Direccion;
    usuario.Hobbies = usuarioActualizado.Hobbies;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE: Eliminar un usuario
app.MapDelete("/usuarios/{id}", async (int id, UsuariosContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario == null) return Results.NotFound();

    db.Usuarios.Remove(usuario);
    await db.SaveChangesAsync();
    return Results.NoContent();
}); */

app.Run();
