using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Classes
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly Dictionary<Type, Object> _repositories = new();
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // Key >> Type [Member]
            // Object >>> NEw GenericRepositoy 

            // TEntity = Member 
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var Repo))
                return (IGenericRepository<TEntity>)Repo;

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
