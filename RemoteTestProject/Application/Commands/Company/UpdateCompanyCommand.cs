using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class UpdateCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public CompanyDTO Model { get; set; }

        public UpdateCompanyCommand(Guid id, CompanyDTO model)
        {
            Id = id;
            Model = model;
        }
    }
}
