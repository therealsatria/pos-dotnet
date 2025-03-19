using Infrastructures.Repositories;
using Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty");
            
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (NotFoundException)
            {
                throw; // Re-throw NotFoundException from repository
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error retrieving {typeof(T).Name}: {ex.Message}", ex, 500);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error retrieving {typeof(T).Name} list: {ex.Message}", ex, 500);
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ValidationException($"{typeof(T).Name} cannot be null");
            
            try
            {
                await _repository.AddAsync(entity);
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiException from repository
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error adding {typeof(T).Name}: {ex.Message}", ex, 500);
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ValidationException($"{typeof(T).Name} cannot be null");
            
            try
            {
                await _repository.UpdateAsync(entity);
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiException from repository
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating {typeof(T).Name}: {ex.Message}", ex, 500);
            }
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty");
            
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (NotFoundException)
            {
                throw; // Re-throw NotFoundException from repository
            }
            catch (ApiException)
            {
                throw; // Re-throw ApiException from repository
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error deleting {typeof(T).Name}: {ex.Message}", ex, 500);
            }
        }
    }
}