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
    public class GetStaffIdHandler : IRequestHandler<GetStaffByIdQuery, StaffReadDTO>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<GetStaffIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetStaffIdHandler(
            ServiceContext context,
            ILogger<GetStaffIdHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }


        public async Task<StaffReadDTO> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to GetStaffIdHandler made to add Staff record");

            try
            {
                var StaffId = request.Id;
                var filter = Builders<Staff>.Filter.Eq("Id", StaffId);
                var model = await this._context.Staffs.Find<Staff>(filter).FirstOrDefaultAsync();
                var modelDTO = _mapper.Map<StaffReadDTO>(model);

                _logger.LogInformation("Call to GetStaffIdHandler completed.");

                return modelDTO;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error fetching Staff.");
                throw new Exception("Error fetching Staff.");
            }
        }
    }
}
