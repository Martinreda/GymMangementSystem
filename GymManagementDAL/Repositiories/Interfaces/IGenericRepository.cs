using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        TEntity? GetById(int Id);
        IEnumerable<TEntity> GetALl(Func<TEntity , bool>Condtion = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
