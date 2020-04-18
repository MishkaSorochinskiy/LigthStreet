using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AzureConnections.Interfaces
{
    public interface IAzureStorageConnection
    {
        string GetConfiguration();
    }
}
