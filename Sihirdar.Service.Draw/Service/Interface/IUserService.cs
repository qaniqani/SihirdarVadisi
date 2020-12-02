using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.Service.Draw.ServiceModel;

namespace Sihirdar.Service.Draw.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserAddResult> Add(UserAddRequest request);
        IEnumerable<UserListResult> List(UserListRequest request);
        DrawUser GetWinUser(UserGetWinRequest request);
        UserWinResult Win(UserWinRequest request);
    }
}