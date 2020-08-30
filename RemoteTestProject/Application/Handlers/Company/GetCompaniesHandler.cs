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
    public class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyReadDTO>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetCompaniesHandler> _logger;
        private readonly IMapper _mapper;

        public GetCompaniesHandler(
            ServiceContext context,
            ILogger<GetCompaniesHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyReadDTO>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetCompaniesHandler made to add company record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    List<CompanyReadDTO> models = new List<CompanyReadDTO>();
                    var results = await this._context.Companies.Find<Company>(session,new BsonDocument()).ToListAsync();                    

                    foreach (var item in results)
                    {
                        var modelDTO = _mapper.Map<CompanyReadDTO>(item);
                        models.Add(modelDTO);




                        var filter_department = Builders<Department>.Filter.Eq("CompanyId", modelDTO.Id);
                        var result_departments = await this._context.Departments.Find<Department>(session, filter_department).ToListAsync();
                        modelDTO.Departments = new List<DepartmentReadDTO>();
                        foreach (var dep in result_departments)
                        {
                            var dep_model = _mapper.Map<DepartmentReadDTO>(dep);
                            modelDTO.Departments.Add(dep_model);


                            var filter_staff = Builders<Staff>.Filter.Eq("DepartmentId", dep_model.Id);
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
                    _logger.LogInformation("Call to GetCompaniesHandler completed.");

                    return models;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error fetching companies.");
                    throw new Exception("Error fetching companies.");
                }
            }
        }
    }
}
