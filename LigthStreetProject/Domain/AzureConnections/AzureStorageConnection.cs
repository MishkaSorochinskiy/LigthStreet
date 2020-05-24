using Domain.AzureConnections.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AzureConnections
{
    public class AzureStorageConnection : IAzureStorageConnection
    {
        private readonly IConfiguration _configuration;

        public AzureStorageConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfiguration()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("AzureStorageKey");
                return connectionString;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
