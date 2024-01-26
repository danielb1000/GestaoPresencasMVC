using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Aluno
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<AlunoUc> AlunoUcs { get; set; } = new List<AlunoUc>();

    public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
