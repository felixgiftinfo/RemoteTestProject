using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class InsertStaffCommand : IRequest<IEnumerable<Guid>>
    {

        public IList<StaffDTO> Models { get; set; }

        public InsertStaffCommand(IList<StaffDTO> models)
        {
            Models = models;
        }
    }
}
