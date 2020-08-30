using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetDepartmentsByCompanyIdQuery : IRequest<IEnumerable<DepartmentReadDTO>>
    {
        public Guid CompanyId { get; set; }

        public GetDepartmentsByCompanyIdQuery(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}
