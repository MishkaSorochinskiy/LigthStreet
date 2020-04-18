using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Root
{
    public interface IRepository<T, Tkey>
        where T : BaseModel
        where Tkey : struct
    {
        Task<List<T>> ToListAsync();
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T> FindByIdAsync(Tkey Id);
    }
}
