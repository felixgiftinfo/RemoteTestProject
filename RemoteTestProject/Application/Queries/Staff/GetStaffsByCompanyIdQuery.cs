using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetStaffsByCompanyIdQuery : IRequest<IEnumerable<StaffReadDTO>>
    {
        public Guid CompanyId { get; set; }

        public GetStaffsByCompanyIdQuery(Guid companyId)
        {
            CompanyId = companyId;
        }
    }
}
