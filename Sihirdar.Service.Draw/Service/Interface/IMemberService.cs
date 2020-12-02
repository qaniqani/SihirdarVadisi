using Sihirdar.Service.Draw.ServiceModel;

namespace Sihirdar.Service.Draw.Service.Interface
{
    public interface IMemberService
    {
        MemberResult Add(MemberRequest request);
        MemberResult Login(MemberLoginRequest request);
        bool Check(MemberCheckApiKeyRequest request, out int id);
    }
}