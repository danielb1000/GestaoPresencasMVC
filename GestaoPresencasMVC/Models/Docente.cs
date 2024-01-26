using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Docente
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<Uc> Ucs { get; set; } = new List<Uc>();
}
