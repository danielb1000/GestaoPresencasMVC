using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPresencasMVC.Models;

public partial class Presenca
{
    public int Id { get; set; }

    public int? IdAula { get; set; }

    public int? IdAluno { get; set; }

    public bool? Presente { get; set; }

    [Display(Name = "Nome do Aluno")]
    [DataType(DataType.Text)]
    public virtual Aluno? IdAlunoNavigation { get; set; }

    [Display(Name = "Aula")]
    [DataType(DataType.Text)]
    public virtual Aula? IdAulaNavigation { get; set; }
}
