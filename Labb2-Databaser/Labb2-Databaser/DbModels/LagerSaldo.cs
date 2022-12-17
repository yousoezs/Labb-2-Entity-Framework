namespace Labb2_Databaser.DbModels;

public partial class LagerSaldo
{
    public int ButikId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? Antal { get; set; }

    public virtual Butiker Butik { get; set; }

    public virtual Böcker IsbnNavigation { get; set; }
}
