using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using SampleWebApiService.DataAccess.Entities;

namespace SampleWebApiService.DataAccess.Repositories
{
    public interface IWriteRepository<T>
    {
        TryAsync<T> CreateAsync(T entity);
        TryOptionAsync<T> UpdateAsync(int id, T entity);
        TryAsync<bool> DeleteByIdAsync(int id);
    }
}
