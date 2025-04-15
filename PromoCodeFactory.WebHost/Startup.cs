using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;

namespace PromoCodeFactory.WebHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // было services.AddScoped
            services.AddSingleton(typeof(IRepository<Employee>), (x) =>
                new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            services.AddSingleton(typeof(IRepository<Role>), (x) =>
                new InMemoryRepository<Role>(FakeDataFactory.Roles));
            services.AddSingleton(typeof(IRepository<Preference>), (x) =>
                new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
            services.AddSingleton(typeof(IRepository<Customer>), (x) =>
                new InMemoryRepository<Customer>(FakeDataFactory.Customers));
            services.AddSingleton(typeof(IRepository<PromoCode>), (x) =>
                new InMemoryRepository<PromoCode>(FakeDataFactory.PromoCodes));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}