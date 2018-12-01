using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        Task<bool> SaveAll();
    }
}
