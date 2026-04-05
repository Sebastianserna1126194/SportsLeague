using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
            .ForMember(
                destination => destination.TeamName,
                options => options.MapFrom(source => source.Team.Name));

        CreateMap<RefereeRequestDTO, Referee>();
        CreateMap<Referee, RefereeResponseDTO>();

        CreateMap<TournamentRequestDTO, Tournament>();
        CreateMap<Tournament, TournamentResponseDTO>()
            .ForMember(
                destination => destination.TeamsCount,
                options => options.MapFrom(source => source.TournamentTeams.Count));

        CreateMap<Sponsor, SponsorResponseDTO>();
        CreateMap<SponsorRequestDTO, Sponsor>();

        CreateMap<TournamentSponsor, TournamentSponsorResponseDTO>()
            .ForMember(dest => dest.TournamentName, opt => opt.MapFrom(src => src.Tournament.Name))
            .ForMember(dest => dest.SponsorName, opt => opt.MapFrom(src => src.Sponsor.Name));

CreateMap<TournamentSponsorRequestDTO, TournamentSponsor>();
    }
    
}
