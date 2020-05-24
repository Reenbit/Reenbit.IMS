using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.IMS.DataAccess.Abstraction
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetById(string id);

        Task<List<TEntity>> GetList();

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task<bool> Delete(string id);
    }
}
