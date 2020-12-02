using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IPromiseDayService : IBaseInterface<PromiseDay>
    {
        PromiseDay GetDayPromise();
    }
}