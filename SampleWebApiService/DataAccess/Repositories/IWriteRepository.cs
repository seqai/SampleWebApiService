using System.Collections.Generic;
using LanguageExt;

namespace SampleWebApiService.DataAccess.Repositories
{
    public interface IReadRepository<T, in TFilter>
    {
        TryOptionAsync<T> GetByIdAsync(int id);
        TryAsync<IList<T>> QueryAsync(TFilter filter);
    }
}
