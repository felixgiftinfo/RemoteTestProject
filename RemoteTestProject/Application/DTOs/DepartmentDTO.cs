using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.DTOs
{
    public class DepartmentDTO
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class DepartmentReadDTO : DepartmentDTO
    {
        public Guid Id { get; set; }
        public IList<StaffReadDTO> Staffs { get; set; }
    }
}
