using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class InsertDepartmentCommand : IRequest<IEnumerable<Guid>>
    {

        public IList<DepartmentDTO> Models { get; set; }

        public InsertDepartmentCommand(IList<DepartmentDTO> models)
        {
            Models = models;
        }
    }
}
