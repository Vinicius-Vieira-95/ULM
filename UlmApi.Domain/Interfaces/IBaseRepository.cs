using UlmApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UlmApi.Domain.Interfaces
{
    public interface IBaseRepository<TEntity, TType> where TEntity : class
    {
        Task Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(TType id);

        Task<IList<TEntity>> Select();

        Task<TEntity> Select(TType id);

        Task Save();
    }
}
