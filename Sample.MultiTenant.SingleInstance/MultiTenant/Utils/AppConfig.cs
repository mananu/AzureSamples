﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.Utils
{
    public class AppConfig
    {
        /// <summary>
        /// Common database settings
        /// </summary>
        public class DatabaseConfig
        {
            public string DatabaseUser { get; set; }
            public string DatabasePassword { get; set; }
            public int DatabaseServerPort { get; set; }
            public int ConnectionTimeOut { get; set; }
            public string LearnHowFooterUrl { get; set; }
        }

        /// <summary>
        /// The Tenant server configs
        /// </summary>
        public class TenantServerConfig
        {
            public string TenantServer { get; set; }
            public string TenantDatabase { get; set; }
            /// <summary>
            /// Boolean value to specify if the events dates need to be reset
            /// This can be set to false when in Development mode
            /// </summary>
            /// <value>
            ///   <c>true</c> if [reset event dates]; otherwise, <c>false</c>.
            /// </value>
            public bool ResetEventDates { get; set; }
        }

        /// <summary>
        /// The tenant configs
        /// </summary>
        public class TenantConfig
        {
            public int TenantId { get; set; }
            public string VenueName { get; set; }
            public string EventTypeNamePlural { get; set; }
            public string BlobImagePath { get; set; }
            public string TenantName { get; set; }
            public string Currency { get; set; }
            public string TenantCulture { get; set; }
            //public List<CountryModel> TenantCountries { get; set; }
            public string User { get; set; }
            public string DatabaseName { get; set; }
            public string DatabaseServerName { get; set; }
        }
    }
}
