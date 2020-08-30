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
    public class GetStaffsByDepartmentIdHandler : IRequestHandler<GetStaffsByDepartmentIdQuery, IEnumerable<StaffReadDTO>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetStaffsByDepartmentIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetStaffsByDepartmentIdHandler(
            ServiceContext context,
            ILogger<GetStaffsByDepartmentIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StaffReadDTO>> Handle(GetStaffsByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetStaffsByDepartmentIdHandler made to add staff record");

            try
            {
                var DepartmentId = request.DepartmentId;
                List<StaffReadDTO> models = new List<StaffReadDTO>();
                var filter = Builders<Staff>.Filter.Eq("DepartmentId", DepartmentId);
                var results = await this._context.Staffs.Find<Staff>(filter).ToListAsync();

                foreach (var item in results)
                {
                    var modelDTO = _mapper.Map<StaffReadDTO>(item);
                    models.Add(modelDTO);
                }

                _logger.LogInformation("Call to GetStaffsByDepartmentIdHandler completed.");
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
