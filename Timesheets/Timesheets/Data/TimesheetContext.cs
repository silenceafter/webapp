using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.Data
{
    public class TimesheetContext : DbContext
    {
        public TimesheetContext(DbContextOptions<TimesheetContext> options) : base(options)
        {
        }

        public class TimesheetContextFactory : IDesignTimeDbContextFactory<TimesheetContext>
        {
            public TimesheetContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<TimesheetContext>();
                optionsBuilder.UseNpgsql(new SqlConnection().GetConnectionString());
                return new TimesheetContext(optionsBuilder.Options);
            }
        }

        public DbSet<CustomerDto> Customers { get; set; }
        public DbSet<ContractDto> Contracts { get; set; }
        public DbSet<TaskDto> Tasks { get; set; }
        public DbSet<InvoiceDto> Invoices { get; set; }
        public DbSet<TaskEmployeeDto> TaskEmployees { get; set; }
        public DbSet<EmployeeDto> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.UseIdentityColumns();
            //tables            
            modelBuilder.Entity<CustomerDto>().ToTable("Customer").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ContractDto>().ToTable("Contract").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TaskDto>().ToTable("Task").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<InvoiceDto>().ToTable("Invoice").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TaskEmployeeDto>().ToTable("TaskEmployee").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<EmployeeDto>().ToTable("Employee").Property(b => b.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
