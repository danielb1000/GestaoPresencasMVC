using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class AlunoUc
{
    public int Id { get; set; }

    public int? IdUc { get; set; }

    public int? IdCurso { get; set; }

    public int? IdAluno { get; set; }

    public virtual Aluno? IdAlunoNavigation { get; set; }

    public virtual Uc? IdUcNavigation { get; set; }
}
