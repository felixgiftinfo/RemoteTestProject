using MediatR;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Commands
{
    public class InsertCompanyCommand : IRequest<IEnumerable<Guid>>
    {

        public IList<CompanyDTO> Models { get; set; }

        public InsertCompanyCommand(IList<CompanyDTO> models)
        {
            Models = models;
        }
    }
}
