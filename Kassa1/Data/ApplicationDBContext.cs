using Kassa1.Properties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class ApplicationDBContext :DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<DebtInfo> DebtInfos { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<InvalidGoods> InvalidGoods { get; set; }
        public DbSet<Kassa> Kassas { get; set; }
        public DbSet<Massa> Massas { get; set; }
        public DbSet<Partiya> Partiyas { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Sold> Solds { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<Vazvrat> Vazvrats { get; set; }
        public DbSet<Worker> Workers { get; set; }


        public ApplicationDBContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(string.Format(@"Server={0};Database={1};User Id={2};Password={3};", Settings.Default.ServerName, Settings.Default.DbName, Settings.Default.UserId, Settings.Default.Password));
        }
    }
}
