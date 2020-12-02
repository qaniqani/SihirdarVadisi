using AdminProject.Models;

namespace AdminProject.Services.Interface
{
    public interface IThirtPartService
    {
        TournamentSaveModelDto GetTournament(int userId);
        bool AddTournament(TournamentSaveModelDto request);
        bool EditTournament(TournamentSaveModelDto request, int userId);
    }
}