using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class UpdateDepartmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DepartmentDTO Model { get; set; }

        public UpdateDepartmentCommand(Guid id, DepartmentDTO model)
        {
            Id = id;
            Model = model;
        }
    }
}
