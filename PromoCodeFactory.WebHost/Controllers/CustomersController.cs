using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;


namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        
        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить данные всех клиентов
        /// </summary>
        /// <returns> Возвращает всех клиентов </returns>
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            //+TODO: Добавить получение списка клиентов
            //throw new NotImplementedException();
            var customers = await _customerRepository.GetAllAsync();

            var customersModelList = customers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,                 
                    Email = x.Email,
                }).ToList();

            return customersModelList;
        }

        /// <summary>
        /// Получить данные клиента по id
        /// </summary>        
        /// <param name="id"> Id запрашиваемого клиента. </param>
        /// <returns> Данные клиента. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerByIdAsync(Guid id)
        {
            //+TODO: Добавить получение клиента вместе с выданными ему промомкодами
            //throw new NotImplementedException();
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            
            CustomerResponse customerModel = new CustomerResponse()
            {
                Id = customer.Id,                
                FirstName=customer.FirstName,
                LastName=customer.LastName,                
                Email = customer.Email, 
                PromoCodes = customer.PromoCodes.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code= x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName,                    
                }).ToList(),
                Preferences = customer.Preferences.Select(x =>
                new Preference()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
            };

            return Ok(customerModel);
        }

        /// <summary>
        /// Добавить данные нового клиента 
        /// </summary>
        /// <param name="request"> Данные нового клиента. </param>
        /// <returns> Ok если добавлен. </returns>
        [HttpPost]
        public async Task<ActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //+TODO: Добавить создание нового клиента вместе с его предпочтениями
            //throw new NotImplementedException();

            //var preferences = await _preferenceRepository.GetAllPrefAsync();
            var preferences = await _preferenceRepository.GetAllAsync();

            List<Preference> CustomerPreferences = new List<Preference> 
                ( from p in preferences
                  join r in request.PreferenceIds on p.Id equals r //into g
                  select new Preference  
                  {
                      Id = p.Id,
                      Name = p.Name,
                  }
                );


            Customer customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Preferences = CustomerPreferences,
            };

            _customerRepository.AddAsync(customer);

            return Ok();
        }

        /// <summary>
        /// Обновить данные клиента 
        /// </summary>
        /// <param name="id"> Id клиента. </param>
        /// <param name="request"> Новые данные клиента. </param>
        /// <returns> Ok если обновлено. </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //+TODO: Обновить данные клиента вместе с его предпочтениями
            //throw new NotImplementedException();
            var customer = await _customerRepository.GetByIdAsync(id);
            //var preferences = await _preferenceRepository.GetAllPrefAsync();
            var preferences = await _preferenceRepository.GetAllAsync();

            if (customer == null)
                return NotFound();

            customer.Id = id;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.Preferences = new List<Preference>
            (from p in preferences
             join r in request.PreferenceIds on p.Id equals r
             select new Preference
             {
                 Id = p.Id,
                 Name = p.Name,
             }
            );                
            _customerRepository.UpdAsync(customer);

            return Ok();

        }

        /// <summary>
        /// Удалить клиента 
        /// </summary>
        /// <param name="id"> Id клиента. </param>        
        /// <returns> Ok если удалено. </returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            //throw new NotImplementedException();
            var customer = await _customerRepository.GetByIdAsync(id);
            _customerRepository.DelAsync(id);
            return Ok();
        }
    }
}