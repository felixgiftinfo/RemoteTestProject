using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.DTOs
{
    public class StaffDTO
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public string StaffNumber { get; set; }
    }

    public class StaffReadDTO : StaffDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
    }
}
