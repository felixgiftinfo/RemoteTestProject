using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
    public class GetStaffsByCompanyIdHandler : IRequestHandler<GetStaffsByCompanyIdQuery, IEnumerable<StaffReadDTO>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetStaffsByCompanyIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetStaffsByCompanyIdHandler(
            ServiceContext context,
            ILogger<GetStaffsByCompanyIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StaffReadDTO>> Handle(GetStaffsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetStaffsByCompanyIdHandler made to add staff record");

            try
            {
                var companyId = request.CompanyId;
                List<StaffReadDTO> models = new List<StaffReadDTO>();
                var filter = Builders<Staff>.Filter.Eq("CompanyId", companyId);
                var results = await this._context.Staffs.Find<Staff>(filter).ToListAsync();

                foreach (var item in results)
                {
                    var modelDTO = _mapper.Map<StaffReadDTO>(item);
                    models.Add(modelDTO);
                }

                _logger.LogInformation("Call to GetStaffsByCompanyIdHandler completed.");
                return models;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error fetching staffs.");
                throw new Exception("Error fetching staffs.");
            }
        }
    }
}
