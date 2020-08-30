using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Context
{
    public interface IServiceSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class ServiceSettings : IServiceSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
