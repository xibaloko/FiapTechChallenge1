using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using System.Linq.Expressions;

namespace FiapTechChallenge.AppService.Services
{
    public class ApplicationServiceBase<T> : IApplicationServiceBase<T> where T : EntityCore
    {
        public IBaseRepository<T> _repository { get; set; }

        public void Add(T entity)
        {
            _repository.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public void AddBulk(IEnumerable<T> entityes)
        {
            _repository.AddBulk(entityes);
        }

        public T Find(int id)
        {
            return _repository.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            return _repository.FirstOrDefault(filter, includeProperties, isTracking);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            return await _repository.FirstOrDefaultAsync(filter, includeProperties, isTracking);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            return _repository.GetAll(filter, orderBy, includeProperties, isTracking);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            return await _repository.GetAllAsync(filter, orderBy, includeProperties, isTracking);
        }

        public void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _repository.RemoveRange(entity);
        }

        public void Save()
        {
            _repository.Save();
        }

        public async Task SaveAsync()
        {
            await _repository.SaveAsync();
        }

        public int SaveCount()
        {
           return _repository.SaveCount();
        }

        public async Task<int> SaveCountAsync()
        {
            return await _repository.SaveCountAsync();
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }
    }
}
