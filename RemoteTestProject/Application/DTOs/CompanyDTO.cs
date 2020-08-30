using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.DTOs
{
    public class CompanyDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }
    public class CompanyReadDTO : CompanyDTO
    {
        public Guid Id { get; set; }
        public IList<DepartmentReadDTO> Departments { get; set; }
    }
}
