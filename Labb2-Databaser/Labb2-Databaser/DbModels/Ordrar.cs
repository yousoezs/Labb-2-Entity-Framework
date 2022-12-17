namespace Labb2_Databaser.DbModels;

public partial class Ordrar
{
    public int OrderId { get; set; }

    public int KundId { get; set; }

    public int? Antal { get; set; }

    public int? StyckPris { get; set; }

    public virtual Kunder Kund { get; set; } = null!;
}
