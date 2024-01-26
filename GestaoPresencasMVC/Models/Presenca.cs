using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Presenca
{
    public int Id { get; set; }

    public int? IdAula { get; set; }

    public int? IdAluno { get; set; }

    public bool? Presente { get; set; }

    public virtual Aluno? IdAlunoNavigation { get; set; }

    public virtual Aula? IdAulaNavigation { get; set; }
}
