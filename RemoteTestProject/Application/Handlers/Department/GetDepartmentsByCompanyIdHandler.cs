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
    public class GetDepartmentsByCompanyIdHandler : IRequestHandler<GetDepartmentsByCompanyIdQuery, IEnumerable<DepartmentReadDTO>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetDepartmentsByCompanyIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetDepartmentsByCompanyIdHandler(
            ServiceContext context,
            ILogger<GetDepartmentsByCompanyIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentReadDTO>> Handle(GetDepartmentsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetDepartmentsByCompanyIdHandler made to add department record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var companyId = request.CompanyId;
                    List<DepartmentReadDTO> models = new List<DepartmentReadDTO>();
                    var filter = Builders<Department>.Filter.Eq("CompanyId", companyId);
                    var results = await this._context.Departments.Find<Department>(session, filter).ToListAsync();

                    foreach (var item in results)
                    {
                        var filter_department = Builders<Staff>.Filter.Eq("DepartmentId", item.Id);
                        var result_departments = await this._context.Staffs.Find<Staff>(session, filter_department).ToListAsync();

                        var modelDTO = _mapper.Map<DepartmentReadDTO>(item);
                        models.Add(modelDTO);

                        modelDTO.Staffs = new List<StaffReadDTO>();
                        foreach (var dep in result_departments)
                        {
                            var dep_model = _mapper.Map<StaffReadDTO>(dep);
                            modelDTO.Staffs.Add(dep_model);
                        }
                    }

                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to GetDepartmentsByCompanyIdHandler completed.");
                    return models;
                }
                catch(Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error fetching departments.");
                    throw new Exception("Error fetching departments.");
                }
            }
        }
    }
}
