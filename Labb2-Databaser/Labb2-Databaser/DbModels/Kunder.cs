using System.Collections.Generic;

namespace Labb2_Databaser.DbModels;

public partial class Kunder
{
    public int Id { get; set; }

    public string? Förnamn { get; set; }

    public string? Efternam { get; set; }

    public virtual ICollection<Ordrar> Ordrars { get; } = new List<Ordrar>();
}
