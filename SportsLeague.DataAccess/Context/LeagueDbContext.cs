using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; }   
    public DbSet<Player> Players { get; set; }
    public DbSet<Referee> Referees { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentTeam> TournamentTeams { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<TournamentSponsor> TournamentSponsors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    ConfigureTeam(modelBuilder);
    ConfigurePlayer(modelBuilder);
    ConfigureReferee(modelBuilder);
    ConfigureTournament(modelBuilder);
    ConfigureTournamentTeam(modelBuilder);
    ConfigureSponsor(modelBuilder);
    ConfigureTournamentSponsor(modelBuilder);
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
private static void ConfigureSponsor(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Sponsor>(entity =>
    {
        entity.HasKey(sponsor => sponsor.Id);

        entity.Property(sponsor => sponsor.Name)
            .IsRequired()
            .HasMaxLength(150);

        entity.Property(sponsor => sponsor.ContactEmail)
            .IsRequired()
            .HasMaxLength(150);

        entity.Property(sponsor => sponsor.Phone)
            .HasMaxLength(30);

        entity.Property(sponsor => sponsor.WebsiteUrl)
            .HasMaxLength(250);

        entity.Property(sponsor => sponsor.Category)
            .IsRequired();

        entity.Property(sponsor => sponsor.CreatedAt)
            .IsRequired();

        entity.Property(sponsor => sponsor.UpdatedAt)
            .IsRequired(false);

        entity.HasIndex(sponsor => sponsor.Name)
            .IsUnique();
    });
}

private static void ConfigureTournamentSponsor(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<TournamentSponsor>(entity =>
    {
        entity.HasKey(tournamentSponsor => tournamentSponsor.Id);

        entity.Property(tournamentSponsor => tournamentSponsor.ContractAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        entity.Property(tournamentSponsor => tournamentSponsor.JoinedAt)
            .IsRequired();

        entity.HasOne(tournamentSponsor => tournamentSponsor.Tournament)
            .WithMany(tournament => tournament.TournamentSponsors)
            .HasForeignKey(tournamentSponsor => tournamentSponsor.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(tournamentSponsor => tournamentSponsor.Sponsor)
            .WithMany(sponsor => sponsor.TournamentSponsors)
            .HasForeignKey(tournamentSponsor => tournamentSponsor.SponsorId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(tournamentSponsor => new { tournamentSponsor.TournamentId, tournamentSponsor.SponsorId })
            .IsUnique();
    });
}
}
