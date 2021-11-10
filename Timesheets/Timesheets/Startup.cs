using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Timesheets.Data;
using Timesheets.DataAccessLayer;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Repositories;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Services
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITaskEmployeeService, TaskEmployeeService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            //Repositories
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ITaskEmployeeRepository, TaskEmployeeRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();

            //EF Core
            services.AddDbContext<TimesheetContext>(options => options.UseNpgsql(new SqlConnection().GetConnectionString()));
            //Mapper
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timesheets", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timesheets v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
