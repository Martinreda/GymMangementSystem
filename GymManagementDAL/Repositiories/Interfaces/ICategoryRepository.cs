using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    internal interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(int Id);
        int Add(Category category);
        int Update(Category category);
        int Delete (Category category);
    }
}
