using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Domains
{
    public class Staff : EntityBase
    {
        public Guid DepartmentId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string StaffNumber { get; set; }
    }
}
