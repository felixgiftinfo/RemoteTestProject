using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetCompaniesQuery : IRequest<IEnumerable<CompanyReadDTO>>
    {
    }
}
