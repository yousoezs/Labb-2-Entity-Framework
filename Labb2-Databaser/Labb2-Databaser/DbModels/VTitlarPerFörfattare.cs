namespace Labb2_Databaser.DbModels;

public partial class VTitlarPerFörfattare
{
    public string Namn { get; set; } = null!;

    public int? Ålder { get; set; }

    public int? Titlar { get; set; }

    public string Lagervärde { get; set; } = null!;
}
