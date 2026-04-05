using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Referee> Referees => Set<Referee>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureTeam(modelBuilder);
        ConfigurePlayer(modelBuilder);
        ConfigureReferee(modelBuilder);
        ConfigureTournament(modelBuilder);
        ConfigureTournamentTeam(modelBuilder);
    }

    private static void ConfigureTeam(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(team => team.Id);

            entity.Property(team => team.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(team => team.City)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(team => team.Stadium)
                .HasMaxLength(150);

            entity.Property(team => team.LogoUrl)
                .HasMaxLength(500);

            entity.Property(team => team.CreatedAt).IsRequired();
            entity.Property(team => team.UpdatedAt).IsRequired(false);

            entity.HasIndex(team => team.Name).IsUnique();
        });
    }

    private static void ConfigurePlayer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(player => player.Id);

            entity.Property(player => player.FirstName)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(player => player.LastName)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(player => player.BirthDate).IsRequired();
            entity.Property(player => player.Number).IsRequired();
            entity.Property(player => player.Position).IsRequired();
            entity.Property(player => player.CreatedAt).IsRequired();
            entity.Property(player => player.UpdatedAt).IsRequired(false);

            entity.HasOne(player => player.Team)
                .WithMany(team => team.Players)
                .HasForeignKey(player => player.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(player => new { player.TeamId, player.Number }).IsUnique();
        });
    }

    private static void ConfigureReferee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(referee => referee.Id);

            entity.Property(referee => referee.FirstName)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(referee => referee.LastName)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(referee => referee.Nationality)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(referee => referee.CreatedAt).IsRequired();
            entity.Property(referee => referee.UpdatedAt).IsRequired(false);
        });
    }

    private static void ConfigureTournament(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(tournament => tournament.Id);

            entity.Property(tournament => tournament.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(tournament => tournament.Season)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(tournament => tournament.StartDate).IsRequired();
            entity.Property(tournament => tournament.EndDate).IsRequired();
            entity.Property(tournament => tournament.Status).IsRequired();
            entity.Property(tournament => tournament.CreatedAt).IsRequired();
            entity.Property(tournament => tournament.UpdatedAt).IsRequired(false);
        });
    }

    private static void ConfigureTournamentTeam(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TournamentTeam>(entity =>
        {
            entity.HasKey(link => link.Id);

            entity.Property(link => link.RegisteredAt).IsRequired();
            entity.Property(link => link.CreatedAt).IsRequired();
            entity.Property(link => link.UpdatedAt).IsRequired(false);

            entity.HasOne(link => link.Tournament)
                .WithMany(tournament => tournament.TournamentTeams)
                .HasForeignKey(link => link.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(link => link.Team)
                .WithMany(team => team.TournamentTeams)
                .HasForeignKey(link => link.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(link => new { link.TournamentId, link.TeamId }).IsUnique();
        });
    }
}
