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
    public class GetCompanyIdHandler : IRequestHandler<GetCompanyByIdQuery, CompanyReadDTO>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetCompanyIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetCompanyIdHandler(
            ServiceContext context,
            ILogger<GetCompanyIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }


        public async Task<CompanyReadDTO> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetCompanyIdHandler made to add company record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var companyId = request.Id;
                    var filter = Builders<Company>.Filter.Eq("Id", companyId);
                    var model = await this._context.Companies.Find<Company>(session, filter).FirstOrDefaultAsync();

                    CompanyReadDTO modelDTO = null;
                    if (model != null)
                    {
                        modelDTO = _mapper.Map<CompanyReadDTO>(model);
                        var filter_department = Builders<Department>.Filter.Eq("CompanyId", modelDTO.Id);
                        var result_departments = await this._context.Departments.Find<Department>(session, filter_department).ToListAsync();
                        modelDTO.Departments = new List<DepartmentReadDTO>();
                        foreach (var dep in result_departments)
                        {
                            var dep_model = _mapper.Map<DepartmentReadDTO>(dep);
                            modelDTO.Departments.Add(dep_model);


                            var filter_staff = Builders<Staff>.Filter.Eq("DepartmentId", dep.Id);
                            var result_staffs = await this._context.Staffs.Find<Staff>(session, filter_staff).ToListAsync();

                            dep_model.Staffs = new List<StaffReadDTO>();
                            foreach (var staff in result_staffs)
                            {
                                var staff_model = _mapper.Map<StaffReadDTO>(staff);
                                dep_model.Staffs.Add(staff_model);
                            }
                        }
                    }
                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to GetCompanyIdHandler completed.");

                    return modelDTO;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error fetching company.");
                    throw new Exception("Error fetching company.");
                }
            }
        }
    }
}
