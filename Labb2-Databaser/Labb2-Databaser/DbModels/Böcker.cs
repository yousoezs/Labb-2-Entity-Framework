using System;
using System.Collections.Generic;

namespace Labb2_Databaser.DbModels;

public partial class Böcker
{
    public string Isbn13 { get; set; } = null!;

    public int? FörfattarId { get; set; }

    public string? Titel { get; set; }

    public string? Språk { get; set; }

    public int? Pris { get; set; }

    public int? Sidor { get; set; }

    public DateTime UtgivningsDatum { get; set; }

    public virtual Författare? Författar { get; set; }

    public virtual ICollection<Förlag> Förlags { get; } = new List<Förlag>();

    public virtual ICollection<LagerSaldo> LagerSaldos { get; } = new List<LagerSaldo>();
}
