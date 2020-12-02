using AdminProject.Services.Models;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IUserService : IBaseInterface<User>
    {
        void SendForgotPassword(string email);
        bool EmailActivation(string activationCode);
        UserStatusTypes ChangeStatus(int id, UserStatusTypes status);
        void ChangePassword(int id, string password);
        void ResetPassword(string code, string email, string password);
        User Login(string email, string password);
        UserActivationTypes UserActive(string activationCode);
        PagerList<User> AllUserList(int skip, int take);
        PagerList<User> ActiveUserList(int skip, int take);
        PagerList<User> AllUserList(UserSearchRequestDto request);
        bool EmailCheck(string email);
        User GetItem(string email);
        void TournamentExport(string path);
    }
}