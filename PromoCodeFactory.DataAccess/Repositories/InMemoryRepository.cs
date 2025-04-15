using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }
       
        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }
                
        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public void AddAsync(T entity)
        {
            var data = Data.ToList();
            data.Add(entity);
            Data = data;
        }

        public void DelAsync(Guid id)
        {
            Data = Data.Where(x => x.Id != id);
        }

        public void UpdAsync(T entity)
        {
            var data = Data.FirstOrDefault(x => x.Id == entity.Id);
            data = entity;
        }

        public Task<Employee> GetByFullNameAsync(string FullName)
        {            
            return Task.FromResult(((IEnumerable<Employee>)Data).FirstOrDefault(x => x.FullName == FullName));           
        }

        public Task<Preference> GetByPreferenceNameAsync(string PreferenceName)
        {            
            return Task.FromResult(((IEnumerable<Preference>)Data).FirstOrDefault(x => x.Name == PreferenceName));
        }
    }
}
