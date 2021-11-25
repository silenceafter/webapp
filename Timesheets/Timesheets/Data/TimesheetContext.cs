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
        public DbSet<UserDto> Users { get; set; }
        public DbSet<RoleDto> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            //tables
            modelBuilder.UseIdentityColumns();            
            //customer
            modelBuilder.Entity<CustomerDto>().ToTable("Customer").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //contract
            modelBuilder.Entity<ContractDto>().ToTable("Contract").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //task
            modelBuilder.Entity<TaskDto>().ToTable("Task").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //invoice
            modelBuilder.Entity<InvoiceDto>().ToTable("Invoice").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //taskEmployee
            modelBuilder.Entity<TaskEmployeeDto>().ToTable("TaskEmployee").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //employee
            modelBuilder.Entity<EmployeeDto>().ToTable("Employee").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            //user
            modelBuilder.Entity<UserDto>().ToTable("User").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserDto>().HasIndex(b => b.Login)
                .IsUnique();

            modelBuilder.Entity<UserDto>().HasData(
                new UserDto() { Id = 1, Login = "silenceafter", Password = "bdw", RoleId = 1 },
                new UserDto() { Id = 2, Login = "mycustomer", Password = "bdw", RoleId = 2 },
                new UserDto() { Id = 3, Login = "myemployee", Password = "bdw", RoleId = 3 }
                );

            //role
            modelBuilder.Entity<RoleDto>().ToTable("Role").Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<RoleDto>().HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<RoleDto>().HasData(
                new RoleDto() { Id = 1, Name = "Administrator" },
                new RoleDto() { Id = 2, Name = "Customer" },
                new RoleDto() { Id = 3, Name = "Employee" }
                );
        }
    }
}
