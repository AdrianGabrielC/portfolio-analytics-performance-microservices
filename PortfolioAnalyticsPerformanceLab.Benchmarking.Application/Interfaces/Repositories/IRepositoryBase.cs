using System.Linq.Expressions;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;

public interface IRepositoryBase<T>
{
    // Retrieves all entities of type T 
    IQueryable<T> GetAll(bool trackChanges = false);

    // Retrieves entities of type T based on provided condition
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}