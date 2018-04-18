using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MultiTenant.Repository
{
    public partial class TenantDbContext : DbContext
    {
        public TenantDbContext(string connectionStr) :
            base(CreateDdrConnection(connectionStr))
        {

        }

        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="connectionStr">The connection string.</param>
        /// <returns></returns>
        private static DbContextOptions CreateDdrConnection(string connectionStr)
        {
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = new SqlConnection(connectionStr);

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;

            return options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
