using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Uc
{
    public int Id { get; set; }

    public int? IdDocente { get; set; }

    public int? IdCurso { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<AlunoUc> AlunoUcs { get; set; } = new List<AlunoUc>();

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Docente? IdDocenteNavigation { get; set; }
}
