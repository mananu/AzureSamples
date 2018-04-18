using MultiTenant.Interfaces;
using MultiTenant.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        #region Private variables

        private readonly string _connectionString;

        #endregion

        #region Constructor

        public TenantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Private methods
        private TenantDbContext CreateContext()
        {
            return new TenantDbContext(_connectionString);
        }
        #endregion
    }
}
