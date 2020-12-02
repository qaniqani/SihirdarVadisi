using System.Collections.Generic;
using Sihirdar.Service.Draw.ServiceModel;

namespace Sihirdar.Service.Draw.Service.Interface
{
    public interface IDefinitionService
    {
        DefinitionAddResult Add(DefinitionAddRequest request);
        IEnumerable<DefinitionListResult> List(DefinitionListRequest request);
        IEnumerable<DefinitionListResult> ActiveList(DefinitionActiveListRequest request);
        DefinitionChangeStatusResult ChangeStatus(DefinitionChangeStatusRequest request);
        UserListResult GetWinUser(DefinitionGetWinUserRequest request);
    }
}