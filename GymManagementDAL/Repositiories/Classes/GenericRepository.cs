using GymManagementDAL.Repositiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using GymManagementDAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;


namespace GymManagementDAL.Repositiories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
         
        

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
      
       

        public IEnumerable<TEntity> GetALl(Func<TEntity, bool> Condtion = null)
        {
            if (Condtion is null)
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
                return _dbContext.Set<TEntity>().AsNoTracking().Where(Condtion).ToList();

        }

        public TEntity? GetById(int Id) => _dbContext.Set<TEntity>().Find(Id);
       

        public void Update(TEntity entity) =>  _dbContext.Set<TEntity>().Update(entity);
        
    }
}
