# Comandos Básicos Para Crear Proyecto De React y Plantilla De .Net

1. Vamos a crear el proyecto de React empleando TypeScript: con
   npx create-next-app@latest
2. Vamos a crear la plantilla de .Net empleando el comando:
   dotnet new webapi -n backend
   
Es importante que tengamos en cuenta que al momento de crear el proyecto de react nos saldrán las opciones de lo que queremos en nuestro proyecto, además de que ese comando nos libera la última versión disponible de react.

# Ya creamos los proyectos, ¿Ahora qué sigue?

Bueno, ahora debemos agregar los comandos en nuestro proyecto de .Net para poder hacer la conexión a nuestra base de datos, para eso usaremos los comandos:

1. dotnet add package Microsoft.EntityFrameworkCore.SqlServer
2. dotnet add package Microsoft.EntityFrameworkCore.Tools.

Una vez que hayamos corrido esos dos comandos en la consola, esperamos a que se terminen de correr.

# ¿Cómo nos vamos a conectar a SQLServer?

Para eso nosotros debemos ir directamente al archivo "appsettings.json" y ahí vamos a agregar la conexión a nuestra base de datos.

```
{
	"ConnectionStrings":{
	"DefaultConnection": "Server= ; Database= ; User Id= ; Password= ;"
	}
}
```

Es importante que sepamos que el código que vamos a ver en el json antes mencionado es:

```
{

  "Logging": {

    "LogLevel": {

      "Default": "Information",

      "Microsoft.AspNetCore": "Warning"

    }

  },

  "AllowedHosts": "*"

}
```

¿Qué es lo que haremos?, sencillamente vamos a agregar una coma al lado de "AllowedHosts" y anexaremos el código que hicimos con anterioridad, resultando de la siguiente manera:

```
{

  "Logging": {

    "LogLevel": {

      "Default": "Information",

      "Microsoft.AspNetCore": "Warning"

    }

  },

  "AllowedHosts": "*",

  "ConnectionStrings":{

  "DefaultConnection": "Server= ; Database= ; User Id= ; Password= ;"

  }

}
```

Como podemos darnos cuenta he dejado intencionalmente algunos espacios en blanco, aquí es dónde vamos a rellenar nuestros datos según la configuración de nuestra conexión de SQLServer.

# ¿Cómo saco esa información de SQLServer?

Por default el "server" lo vamos a obtener del nombre de la máquina.

La base de datos ("Database"): es la que nosotros creamos y haremos referencia.

El "User Id" Es el valor que vemos en login.

En cuánto a "Password": Es la contraseña con la que ingresamos, en mi caso el código quedaría:

```
{

  "Logging": {

    "LogLevel": {

      "Default": "Information",

      "Microsoft.AspNetCore": "Warning"

    }

  },

  "AllowedHosts": "*",

  "ConnectionStrings": {

    "DefaultConnection": "Server=ALI_G;Database=Usuarios;User Id=Guillermo_G;Password=Alisson240904;"

  }

}
```

# Momento de crear nuestro Modelo

¿Cómo haremos eso?

Antes de empezar a usar los comandos para esto debemos asegurarnos de instalar en el CMD del sistema o en el proyecto el siguiente comando:

1. dotnet tool install --global dotnet-ef

Verificamos:

2. dotnet ef

Para eso usaremos el siguiente comando

1. dotnet ef dbcontext scaffold "Server=ALI_G;Database=Usuarios;User Id=Guillermo_G;Password=Alisson240904;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models

En dónde:

dotnet ef = Herramienta que ejecuta el comando.

dbcontext scaffold = Hace el esqueleto del DbContext y sus modelos correspondientes.

"Server=ALI_G;Database=Usuarios;User Id=Guillermo_G;Password=Alisson240904;TrustServerCertificate=True" = Información de conexión en dónde activamos el modo de confianza, esto es para entornos locales, en producción se necesita configurar el certificado ssl.

Microsoft.EntityFrameworkCore.SqlServer = Proveedor de datos para SQLServer.

--output-dir Models = Especifica el nombre del directorio dónde se van a generar las clases de modelo y el DBContext.

# Ahora debemos empezar a construir la lógica

Recordemos que estamos utilizando una plantilla, la cuál se creó con el comando:

1. dotnet new webapi -n backend

Por lo tanto debemos modificar el "Program.cs", para que este entienda que ahora tienen que hacerle caso al contexto de nuestra base de datos, para eso haremos las siguientes modificación:

1. Instalamos: dotnet add package Microsoft.EntityFrameworkCore.
2. En "Program.cs" nos aseguramos de poder traer las dependencias que necesitamos y de construir los servicios:
   
```
using backend.Models;  

using Microsoft.EntityFrameworkCore;

  

var builder = WebApplication.CreateBuilder(args);

  

// Agrega el contexto de la base de datos (UsuariosContext) al contenedor de servicios

builder.Services.AddDbContext<UsuariosContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

  
var app = builder.Build();

app.MapGet("/usuarios", async (UsuariosContext db) =>
{
    return await db.Usuarios.ToListAsync();
});  

app.Run();
```

# Ahora vamos a revisar si todo funciona de manera correcta.

Para eso podemos ingresar a la ruta:
## http://localhost:5209/usuarios

Ya sea desde el navegador o desde PostMan.


# Continuando con los demás métodos

Ahora que hemos visto el listado ahora debemos ver el resto de las funciones:

```
// POST: Crear un nuevo usuario

app.MapPost("/usuarios", async (UsuariosContext db, Usuario nuevoUsuario) => { db.Usuarios.Add(nuevoUsuario); await db.SaveChangesAsync(); return Results.Created($"/usuarios/{nuevoUsuario.Id}", nuevoUsuario); });

// PUT: Actualizar un usuario existente 

app.MapPut("/usuarios/{id}", async (int id, UsuariosContext db, Usuario usuarioActualizado) => { var usuario = await db.Usuarios.FindAsync(id); if (usuario == null) return Results.NotFound(); usuario.Nombre = usuarioActualizado.Nombre; usuario.Edad = usuarioActualizado.Edad; usuario.EsEstudiante = usuarioActualizado.EsEstudiante; usuario.Direccion = usuarioActualizado.Direccion; usuario.Hobbies = usuarioActualizado.Hobbies; await db.SaveChangesAsync(); return Results.NoContent(); });

// DELETE: Eliminar un usuario

app.MapDelete("/usuarios/{id}", async (int id, UsuariosContext db) => { var usuario = await db.Usuarios.FindAsync(id); if (usuario == null) return Results.NotFound(); db.Usuarios.Remove(usuario); await db.SaveChangesAsync(); return Results.NoContent(); });
```

# ¿Y ahora cómo lo probamos?

Para el método get, sencillamente debemos ir a postman y escribir:

1. http://localhost:5209/usuarios

Para el método post, debemos escribir:

1. http://localhost:5209/usuarios

Y debemos poner en el body:

```
{

    "nombre": "Maria",

    "edad": 22,

    "esEstudiante": false,

    "direccion": "Av. Siempre Viva 123",

    "hobbies": "Leer libros"

}
```

Para el método put, será la ruta:

1. http://localhost:5209/usuarios/2

Y en el body:

```
    {

        "id": 2,

        "nombre": "Maria",

        "edad": 22,

        "esEstudiante": true,

        "direccion": "Av. Siempre Viva 123",

        "hobbies": "Leer libros"

    }
```

Para el método delete, iremos a la ruta:

1. http://localhost:5209/usuarios/2

De esta manera podremos probar nuestro crud


> [!NOTE] Parte EXPERIMENTAL

# Expandiendo la lógica de negocios.

Hemos usado el modelo de esa manera ya que directamente hemos decidido solo mostrar datos, por lo tanto podríamos usar solo el modelo, sin embargo si deseamos expandir la lógica de negocios tenemos que guíarnos en el principio de una única responsabilidad por función o modelo.

En lugar de trabajar solamente en el modelo, podemos crear un controlador que se encargue de llevar cosas más complejas como agregar usuarios, editarlos o eliminarlos, haciendo uso del MVC.

Suena bonito, pero,  ¿cómo vamos a hacerlo?, para eso es necesario que corramos el siguiente comando:

1. dotnet new apicontroller -n UsuariosController -o Controllers -ac

En dónde:

Dotnet es el comando, con "new apicontroller" le decimos que es un controlador cuyo nombre será "UsuariosController" y que se va a crear en una ruta llamada "Controllers".

-n = -name.
-o = -output.
-ac = controlador con funciones precargadas.

Ahora, ¿Cómo vamos a modificar al constructor para que se adapte a la nueva lógica?.

Recordemos que "Program.cs" estaba cómo:

```
using backend.Models;  

using Microsoft.EntityFrameworkCore;

  

var builder = WebApplication.CreateBuilder(args);

  

// Agrega el contexto de la base de datos (UsuariosContext) al contenedor de servicios

builder.Services.AddDbContext<UsuariosContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

  

var app = builder.Build();

  

app.MapGet("/usuarios", async (UsuariosContext db) =>

{

    return await db.Usuarios.ToListAsync();

});

  
  

app.Run();
```

Ahora vamos a modificarlo para que en lugar de ir directamente la ruta con la función asíncrona ahí, llamemos al controlador:

```
// Habilitar controladores
builder.Services.AddControllers();

var app = builder.Build();

// Registrar los controladores
app.MapControllers(); app.Run();
```

# Manejando en controlador creado

Ya hemos creado de manera exitosa al controlador "UsuariosController.cs", y ya hemos hecho que el constructor pueda cargar esto, ahora lo siguiente sería ir desglosando cada función del controlador, veamos más de cerca al get, el cuál actualmente viene cómo:

```
        // GET: api/<UsuariosController>

        [HttpGet]

        public IEnumerable<string> Get()

        {

            return new string[] { "value1", "value2" };

        }
```

¿Qué es lo que vamos a hacer?

Vamos a obtener todos los registros de la base de datos, para eso vamos a modificar un poco el código.

```
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Usuario>>> Get(UsuariosContext db){
	var = usuarios await db.Usuario.TolistAsync();
	return Ok(usuarios);
	}
```

Básicamente lo que estamos haciendo es crear una función pública que es una tarea que se va a ejecutar en forma de lista, en este caso se están listando los usuarios, y gracias a que estamos usando "ActionResult" sabemos que la respuesta que esperamos es un "200 OK", entonces vamos a seguir la ejecución normal del servidor indicando que es asíncrono, mientras se recuperan los datos, de esta manera si ocurre un error al traerlos, en teoría no debería afectar el flujo de la aplicación, y en caso de ser correcta la respuesta, se retornan los usuarios listados. 

# Indexando Swagger

Antes de comenzar con esta parte debemos de saber:

## ¿Qué es Swagger?

Swagger es una herramienta que facilita la manera en la que externamos cómo funcionan nuestras APIS, básicamente nos ayuda a documentarlas, por lo que podemos ocuparnos de tener más cuidado con nuestros proyectos.

Una vez que sepamos que es esta herramienta, podemos usarla para que nuestra API sea más legible para cualquier persona que desee usarla.

La manera de agregarlo a nuestro proyecto es la siguiente:

1. Creamos un certificado https que sirva para nuestro entorno de desarrollo, empleando el comando ==dotnet dev-certs https --trust==
2. Ahora en nuestro constructor o "Program.cs", vamos a indexar un fragmento de código antes de la parte de build: 

```
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();
```

3. Vamos a agregar el entorno de Swagger:

```
if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI();

}

  

app.UseHttpsRedirection();

  

app.UseAuthorization();

  

app.MapControllers();
```

4. Ahora podemos correr nuestro proyecto con el comando ==dotnet run --launch-profile https==

5. Vamos a la ruta https://localhost:5209/swagger/index.html 

En dónde 

:5209 = El puerto que nos genera el comando dotnet run
/swagger = La parte que nos llevará a la UI de swagger dónde vamos a poder ver nuestras apis

# Códigos

Si hemos llegado hasta esta parte el código que tenemos debería ser el siguiente:

## Program.cs

```
using backend.Models;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsuariosContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();  

app.MapControllers();  

app.Run();
```

Y el código que más creamos conveniente en nuestros modelos y controladores basándonos en la lógica de nuestro negocio
# Errores y Soluciones

## ¿Qué hacer si la ruta no carga?

Es probable que hayamos cometido algún error al momento de crear las carpetas, para eso tenemos que fijarnos en la manera que estamos estructurando el proyecto, en este caso es un trabajo local, por lo que vemos:

Directorio: C:\Users\guill\OneDrive\Desktop\DotNet


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----     14/09/2024  08:58 p. m.                backend       
-a----     13/09/2024  07:10 a. m.           1123 DotNet.sln    
-a----     13/09/2024  02:45 p. m.             10 README.md     

Yo tengo en mi carpeta "DotNet" una subcarpeta llamada "backend"

    Directorio: C:\Users\guill\OneDrive\Desktop\DotNet\backend   


Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d-----     13/09/2024  07:10 a. m.                bin
d-----     13/09/2024  11:41 a. m.                Controllers    
d-----     13/09/2024  07:49 a. m.                Models
d-----     13/09/2024  02:06 p. m.                obj
d-----     13/09/2024  04:47 a. m.                Properties     
-a----     13/09/2024  04:47 a. m.            127 appsettings.De
                                                  velopment.json 
-a----     13/09/2024  06:30 a. m.            286 appsettings.js 
                                                  on
-a----     13/09/2024  02:06 p. m.            838 backend.csproj 
-a----     13/09/2024  04:47 a. m.            127 backend.http   
-a----     14/09/2024  09:20 p. m.           2081 Program.cs     

Es en "backend" donde tengo toda la parte lógica de mi API, debemos supervisar que la carpeta de nuestro controlador y nuestro modelo estén dentro de nuestra carpeta principal.

Si todo está bien debemos supervisar si nuestra lógica del controlador y del modelo son correctas.
