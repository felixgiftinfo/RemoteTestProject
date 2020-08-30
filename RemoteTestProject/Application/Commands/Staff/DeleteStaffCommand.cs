using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class DeleteStaffCommand : IRequest<long>
    {
        public Guid Id { get; set; }

        public DeleteStaffCommand(Guid id)
        {
            Id = id;
        }
    }
}
