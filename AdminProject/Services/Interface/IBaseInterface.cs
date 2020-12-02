using System.Collections.Generic;

namespace AdminProject.Services.Interface
{
    public interface IBaseInterface<T> where T : class
    {
        void Add(T instance);
        void Edit(int id, T newInstance);
        void Delete(int id);
        IList<T> List();
        T GetItem(int instanceId);
    }
}