using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class UpdateStaffCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public StaffDTO Model { get; set; }

        public UpdateStaffCommand(Guid id, StaffDTO model)
        {
            Id = id;
            Model = model;
        }
    }
}
