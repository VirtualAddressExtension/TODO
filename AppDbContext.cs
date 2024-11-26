using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO
{
    public class AppDbContext : DbContext
    {
        private static string SqlConnectionIntegratedSecurity
        {
            get
            {
                var sb = new SqlConnectionStringBuilder
                {
                    DataSource = "kyserv",
                    InitialCatalog = "ISP-43-Khadzhiev",
                    IntegratedSecurity = true,
                    TrustServerCertificate = true,
                };

                return sb.ConnectionString;
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SqlConnectionIntegratedSecurity);
        }
    }
}
