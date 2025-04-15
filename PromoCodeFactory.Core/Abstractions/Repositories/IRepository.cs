using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
                
        Task<T> GetByIdAsync(Guid id);

        void AddAsync(T entity);

        void DelAsync(Guid id);

        void UpdAsync(T entity);

        Task<Employee> GetByFullNameAsync(string FullName);

        Task<Preference> GetByPreferenceNameAsync(string PreferenceName);
    }
}