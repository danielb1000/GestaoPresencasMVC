using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPresencasMVC.Models;

public partial class AlunoUc
{
    public int Id { get; set; }

    public int? IdUc { get; set; }

    public int? IdCurso { get; set; }

    public int? IdAluno { get; set; }

    [Display(Name = "Nome do Aluno")]
    [DataType(DataType.Text)]
    public virtual Aluno? IdAlunoNavigation { get; set; }

    [Display(Name = "Nome da UC")]
    [DataType(DataType.Text)]
    public virtual Uc? IdUcNavigation { get; set; }
}
