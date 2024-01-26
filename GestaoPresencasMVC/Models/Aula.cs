using System;
using System.Collections.Generic;

namespace GestaoPresencasMVC.Models;

public partial class Aula
{
    public int Id { get; set; }

    public int? IdUc { get; set; }

    public int? IdAno { get; set; }

    public DateOnly? Data { get; set; }

    public string? Sala { get; set; }

    public virtual Ano? IdAnoNavigation { get; set; }

    public virtual Uc? IdUcNavigation { get; set; }

    public virtual ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
