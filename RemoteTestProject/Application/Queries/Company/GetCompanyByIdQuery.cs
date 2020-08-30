using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetCompanyByIdQuery : IRequest<CompanyReadDTO>
    {
        public Guid Id { get; set; }

        public GetCompanyByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
