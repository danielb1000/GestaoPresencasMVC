using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPresencasMVC.Models;

public partial class Aula
{
    public int Id { get; set; }

    public int? IdUc { get; set; }

    public int? IdAno { get; set; }

    public DateOnly? Data { get; set; }

    public string? Sala { get; set; }

    [Display(Name = "Ano Letivo")]
    [DataType(DataType.Text)]
    public virtual Ano? IdAnoNavigation { get; set; }

    [Display(Name = "Nome da Uc")]
    [DataType(DataType.Text)]
    public virtual Uc? IdUcNavigation { get; set; }

    public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
