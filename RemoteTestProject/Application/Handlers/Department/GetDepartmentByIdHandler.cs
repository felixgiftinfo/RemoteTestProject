using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using RemoteTestProject.Application.Commands;
using RemoteTestProject.Application.Domains;
using RemoteTestProject.Application.DTOs;
using RemoteTestProject.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Handlers
{
    public class GetDepartmentIdHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentReadDTO>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetDepartmentIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetDepartmentIdHandler(
            ServiceContext context,
            ILogger<GetDepartmentIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }


        public async Task<DepartmentReadDTO> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetDepartmentIdHandler made to add department record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var departmentId = request.Id;
                    var filter = Builders<Department>.Filter.Eq("Id", departmentId);
                    var model = await this._context.Departments.Find<Department>(session, filter).FirstOrDefaultAsync();

                    _logger.LogInformation("Call to GetDepartmentIdHandler completed.");

                    DepartmentReadDTO modelDTO = null;
                    if (model != null)
                    {
                        modelDTO = _mapper.Map<DepartmentReadDTO>(model);
                        var filter_staff = Builders<Staff>.Filter.Eq("DepartmentId", modelDTO.Id);
                        var result_staffs = await this._context.Staffs.Find<Staff>(session, filter_staff).ToListAsync();

                        modelDTO.Staffs = new List<StaffReadDTO>();
                        foreach (var dep in result_staffs)
                        {
                            var dep_model = _mapper.Map<StaffReadDTO>(dep);
                            modelDTO.Staffs.Add(dep_model);
                        }
                    }

                    await session.CommitTransactionAsync();
                    return modelDTO;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error fetching department.");
                    throw new Exception("Error fetching department.");
                }
            }
        }
    }
}
