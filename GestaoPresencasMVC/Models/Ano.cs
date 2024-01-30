using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPresencasMVC.Models;

public partial class Ano
{
    public int Id { get; set; }

    [Display(Name = "Ano Letivo")]
    [DataType(DataType.Text)]
    public int? Numero { get; set; }

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();
}
