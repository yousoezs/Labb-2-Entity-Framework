using System.Collections.Generic;

namespace Labb2_Databaser.DbModels;

public partial class Butiker
{
    public int Id { get; set; }

    public string? Butik { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldos { get; } = new List<LagerSaldo>();
}
