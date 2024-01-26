using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Ano
{
    public int Id { get; set; }

    public int? Numero { get; set; }

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();
}
