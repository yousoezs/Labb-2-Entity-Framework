namespace Labb2_Databaser.DbModels;

public partial class Förlag
{
    public int Id { get; set; }

    public string Isbn13 { get; set; } = null!;

    public string? Företag { get; set; }

    public virtual Böcker Isbn13Navigation { get; set; } = null!;
}
