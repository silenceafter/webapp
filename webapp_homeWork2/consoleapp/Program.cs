using Microsoft.EntityFrameworkCore;
using System;

namespace consoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            // получение данных
            using (ApplicationContext db = new ApplicationContext())
            {
                // получаем объекты из бд и выводим на консоль
                
                /*var users = db.Users.ToList();
                Console.WriteLine("Users list:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                }*/
            }
        }
    }

    public class ApplicationContext : DbContext
    {
        //public DbSet<Table1> Users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=kittendb;Username=postgres;Password=bdw");
        }
    }
}
