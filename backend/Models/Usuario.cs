using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public bool EsEstudiante { get; set; }

    public string Direccion { get; set; } = null!;

    public string? Hobbies { get; set; }
}
