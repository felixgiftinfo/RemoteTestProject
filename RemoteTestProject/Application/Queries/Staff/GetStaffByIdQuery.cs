using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Queries
{
    public class GetStaffByIdQuery : IRequest<StaffReadDTO>
    {
        public Guid Id { get; set; }

        public GetStaffByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
