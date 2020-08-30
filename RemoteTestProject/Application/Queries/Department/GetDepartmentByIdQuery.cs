using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetDepartmentByIdQuery : IRequest<DepartmentReadDTO>
    {
        public Guid Id { get; set; }

        public GetDepartmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
