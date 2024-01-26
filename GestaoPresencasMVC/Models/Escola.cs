using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Escola
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
