using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Application.Interfaces.Repositories;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Repositories.BaseRepository;

public class RepositoryBase<T>: IRepositoryBase<T> where T: class
{
    protected BenchmarkingDbContext _context;    
    
    public RepositoryBase(BenchmarkingDbContext dbContext) => this._context = dbContext;

    public IQueryable<T> GetAll(bool trackChanges) =>
        !trackChanges
            ? _context.Set<T>().AsNoTracking()
            : _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => 
        !trackChanges ?
            _context.Set<T>().Where(expression).AsNoTracking()
            : _context.Set<T>().Where(expression);
    
    public void Create(T entity) => _context.Set<T>().Add(entity);
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}