using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPresencasMVC.Models;

public partial class Curso
{
    public int Id { get; set; }

    public int? IdEscola { get; set; }

    public string? Nome { get; set; }

    [Display(Name = "Nome da Escola")]
    [DataType(DataType.Text)]
    public virtual Escola? IdEscolaNavigation { get; set; }

    public virtual ICollection<Uc> Ucs { get; set; } = new List<Uc>();
}
