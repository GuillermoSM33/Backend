using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models; 
using Microsoft.EntityFrameworkCore; 

namespace backend.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosContext _context;

        public UsuariosController(UsuariosContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // GET api/usuarios/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/usuarios
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null)
            {
                return BadRequest("El usuario no puede ser nulo.");
            }

            try
            {
                using (var db = new UsuariosContext())
                {
                    db.Usuarios.Add(nuevoUsuario);
                    await db.SaveChangesAsync();
                }
                return Ok("Usuario agregado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hubo un error al agregar el usuario: {ex.Message}");
            }
        }

        // PUT api/usuarios/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/usuarios/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

