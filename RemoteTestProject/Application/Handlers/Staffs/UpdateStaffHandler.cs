using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using RemoteTestProject.Application.Commands;
using RemoteTestProject.Application.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Handlers
{
    public class UpdateStaffHandler : IRequestHandler<UpdateStaffCommand, bool>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<UpdateStaffHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateStaffHandler(
            ServiceContext context,
            ILogger<UpdateStaffHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to UpdateStaffHandler made to update staff record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var StaffId = request.Id;

                    var model = await this._context.Staffs.FindSync(session, Builders<Staff>.Filter.Eq("Id", StaffId)).FirstOrDefaultAsync();
                    if (model == null)
                    {
                        _logger.LogInformation("Staff not found.");
                        throw new Exception("Staff not found.");
                    }

                    var department = await this._context.Departments.FindSync(session, Builders<Department>.Filter.Eq("Id", request.Model.DepartmentId)).FirstOrDefaultAsync();
                    if (department == null)
                    {
                        _logger.LogInformation("Department in staff not found.");
                        throw new Exception("Department in staff not found.");
                    }

                    _mapper.Map(request.Model, model);
                    model.CompanyId = department.CompanyId;

                    var filter = Builders<Staff>.Filter.Eq("Id", StaffId);
                    var result = await this._context.Staffs.ReplaceOneAsync(session, filter, model, new ReplaceOptions() { IsUpsert = true });


                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to UpdateStaffHandler completed.");
                    return result.IsAcknowledged && result.ModifiedCount > 0;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
