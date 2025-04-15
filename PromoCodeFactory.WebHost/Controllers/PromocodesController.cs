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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository, IRepository<Employee> employeeRepository, IRepository<Preference> preferenceRepository, IRepository<Customer> customerRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _employeeRepository = employeeRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;

        }



        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            //+TODO: Получить все промокоды 
            //throw new NotImplementedException();
            var promoCode = await _promoCodeRepository.GetAllAsync();

            var promoCodeModelList = promoCode.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName,
                }).ToList();

            return Ok(promoCodeModelList);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            //throw new NotImplementedException();
            
            var employee = await _employeeRepository.GetByFullNameAsync(request.PartnerName);
            var preference = await _preferenceRepository.GetByPreferenceNameAsync(request.Preference);
            var customer = await _customerRepository.GetAllAsync();
                        
            var Customers = customer.Where(x => x.Preferences.Any(y => y.Id == preference.Id));
            
            foreach (Customer Custom in Customers)
            {
                PromoCode promoCode = new PromoCode()
                {
                    Id = Guid.NewGuid(),
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(10),
                    PartnerName = request.PartnerName,
                    PartnerManager = employee,
                    Preference = preference,
                    Customer = Custom,
                };
                _promoCodeRepository.AddAsync(promoCode);
            }
            return Ok();
        }
    }
}