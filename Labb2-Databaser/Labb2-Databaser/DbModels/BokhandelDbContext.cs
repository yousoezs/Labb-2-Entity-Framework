using Microsoft.EntityFrameworkCore;

namespace Labb2_Databaser.DbModels;

public partial class BokhandelDbContext : DbContext
{
    public BokhandelDbContext()
    {
    }

    public BokhandelDbContext(DbContextOptions<BokhandelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Förlag> Förlags { get; set; }

    public virtual DbSet<Kunder> Kunders { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

    public virtual DbSet<Ordrar> Ordrars { get; set; }

    public virtual DbSet<VTitlarPerFörfattare> VTitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-4M10KVE0;Initial Catalog=BokhandelDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Butiker__3214EC2736F840B5");

            entity.ToTable("Butiker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress).IsUnicode(false);
            entity.Property(e => e.Butik).IsUnicode(false);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Böcker__3BF79E039F4E5330");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .HasColumnName("ISBN13");
            entity.Property(e => e.FörfattarId).HasColumnName("FörfattarID");
            entity.Property(e => e.Språk).IsUnicode(false);
            entity.Property(e => e.Titel).IsUnicode(false);

            entity.HasOne(d => d.Författar).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörfattarId)
                .HasConstraintName("FK__Böcker__Författa__4222D4EF");
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC2769723AD1");

            entity.ToTable("Författare");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Efternamn).IsUnicode(false);
            entity.Property(e => e.Födelsedatum).HasColumnType("date");
            entity.Property(e => e.Förnamn).IsUnicode(false);
        });

        modelBuilder.Entity<Förlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Förlag__3214EC271A558003");

            entity.ToTable("Förlag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Företag).IsUnicode(false);
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .HasColumnName("ISBN13");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Förlags)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Förlag__ISBN13__5CD6CB2B");
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kunder__3214EC27728FC7B0");

            entity.ToTable("Kunder");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Efternam).IsUnicode(false);
            entity.Property(e => e.Förnamn).IsUnicode(false);
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn }).HasName("PK__LagerSal__1191B8942DB7830D");

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSald__Butik__46E78A0C");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSaldo__ISBN__47DBAE45");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Ordrar__C3905BAF9C561485");

            entity.ToTable("Ordrar");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.KundId).HasColumnName("KundID");

            entity.HasOne(d => d.Kund).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.KundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ordrar__KundID__30F848ED");
        });

        modelBuilder.Entity<VTitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vTitlarPerFörfattare");

            entity.Property(e => e.Lagervärde)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Namn).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
